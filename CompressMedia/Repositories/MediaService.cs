using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Enums;
using CompressMedia.Exceptions;
using CompressMedia.Exceptions.BaseException;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
	public class MediaService : IMediaService
	{
		public readonly ApplicationDbContext _context;
		public readonly BlobStorageDbContext _storageContext;
		public readonly IGridFSBucket _gridFSBucket;
		public MediaService(ApplicationDbContext context, BlobStorageDbContext storageContext)
		{
			_context = context;
			_storageContext = storageContext ?? throw new ArgumentNullException(nameof(storageContext));
			_gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
		}

		/// <summary>
		/// Đổi định djang file
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public string ChangeFileExtension(string extension)
		{
			return Path.ChangeExtension(Path.GetTempFileName(), extension);
		}

		/// <summary>
		/// Xóa blob cũ (trong sql server và gridFs)
		/// </summary>
		/// <param name="oldBlob"></param>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		public async Task DeleteOldBlob(Blob oldBlob, BlobDto blobDto)
		{
			//Xóa video cũ trong sql server
			if (oldBlob != null)
			{
				_context.blobs.Remove(oldBlob);
				await _context.SaveChangesAsync();
			}

			//Xóa video cũ trong mongodb
			var oldFileId = new ObjectId(blobDto.BlobId);
			var filter = Builders<GridFSFileInfo<ObjectId>>.Filter.Eq(x => x.Id, oldFileId);
			var oldFileEntry = await _gridFSBucket.FindAsync(filter);
			var oldFile = oldFileEntry.FirstOrDefault();
			if (oldFile != null)
			{
				await _gridFSBucket.DeleteAsync(oldFile.Id);
			}
		}

		/// <summary>
		/// Xóa các file tạm được tạo ra
		/// </summary>
		public void DeleteTemporaryFiles()
		{
			string tempDirectory = Path.GetTempPath();

			string[] tempFiles = Directory.GetFiles(tempDirectory, "*.tmp");
			string[] mp4Files = Directory.GetFiles(tempDirectory, "*.mp4");
			string[] jpgFiles = Directory.GetFiles(tempDirectory, "*.jpg");

			foreach (string file in tempFiles.Concat(mp4Files).Concat(jpgFiles))
			{
				try
				{
					File.Delete(file);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error deleting file {file}: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// Download file về máy
		/// </summary>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		public async Task<string> DownloadFile(BlobDto blobDto)
		{
			var filter = Builders<GridFSFileInfo<ObjectId>>
								  .Filter.Eq(x => x.Id, ObjectId.Parse(blobDto.BlobId));
			var fileEntry = (await _gridFSBucket.FindAsync(filter)).FirstOrDefault();

			if (fileEntry is null)
				return null!;

			byte[] content = await _gridFSBucket.DownloadAsBytesAsync(fileEntry.Id);

			string tempInputPath = Path.GetTempFileName();
			using (FileStream fileStream = new FileStream(tempInputPath, FileMode.Create, FileAccess.Write))
			{
				await fileStream.WriteAsync(content, 0, content.Length);
			}
			return tempInputPath;
		}

		/// <summary>
		/// Thực thi command
		/// </summary>
		/// <param name="arguments"></param>
		public void ExecuteCommand(string arguments)
		{

			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "ffmpeg",
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true
			};

			using (Process process = new Process())
			{
				process.StartInfo = psi;

				// Show ra output hoặc error
				process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
				process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

				// Chạy process
				process.Start();

				// Đọc output hoặc error
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				// Chờ cho kết thúc
				process.WaitForExit();
			}
		}

		/// <summary>
		/// Chọn option nén image
		/// </summary>
		/// <param name="imageInput"></param>
		/// <param name="outputPath"></param>
		/// <param name="imageManipulationMode"></param>
		/// <param name="imageZoomMode"></param>
		/// <param name="aspectRatioMode"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public string GenerateImageCompressionCommand(string imageInput, string outputPath, ImageManipulationMode? imageManipulationMode, ImageZoomMode? imageZoomMode, ImageAspectRatioMode? aspectRatioMode)
		{
			return imageManipulationMode switch
			{
				ImageManipulationMode.ZoomOut or ImageManipulationMode.ZoomIn when imageZoomMode != null =>
					CompressOption.ImageManipulationOptions[$"{imageManipulationMode}_{(imageZoomMode == ImageZoomMode.Min ? "1.5" : "2")}"].Replace("{imagePath}", imageInput).Replace("{outputPath}", outputPath),

				ImageManipulationMode.AspectRatio when aspectRatioMode != null =>
					CompressOption.ImageManipulationOptions[$"AspectRatio_{aspectRatioMode.ToString()!.Replace("Aspect", string.Empty)}"].Replace("{imagePath}", imageInput).Replace("{outputPath}", outputPath),

				ImageManipulationMode.Compress =>
					CompressOption.ImageManipulationOptions[$"{imageManipulationMode}"].Replace("{imagePath}", imageInput).Replace("{outputPath}", outputPath),

				_ => throw new ArgumentException("Invalid resize option")
			};
		}

		/// <summary>
		/// Chọn option nén video.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="videoPath"></param>
		/// <param name="fileNameOutput"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>  
		public string GenerateVideoCompressionCommand(string videoPath, string outputPath, int width, int height, string fps, bool isFps, bool isResolution, bool isBitrate)
		{
			string key = $"{width}x{height}_{fps}_{(isFps ? "true" : "false")}Fps_{(isResolution ? "true" : "false")}Resolution_{(isBitrate ? "true" : "false")}Bitrate";

			// Chỉ nén fps
			if (isFps && !isResolution && !isBitrate && CompressOption.FpsOnlyOption.TryGetValue(key, out string? fpsCommand))
				return fpsCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			// Chỉ nén resolution
			if (isResolution && !isFps && !isBitrate && CompressOption.ResolutionOnlyOption.TryGetValue(key, out string? resolutionCommand))
				return resolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			// Chỉ nén bitrate
			if (isBitrate && !isFps && !isResolution && CompressOption.BitrateOnlyOption.TryGetValue(key, out string? bitrateCommand))
				return bitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			// Nén fps và resolution
			if (!isBitrate && isFps && isResolution && CompressOption.FpsVsResolutionOption.TryGetValue(key, out string? fpsVsResolutionCommand))
				return fpsVsResolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			// Nén fps và bitrate
			if (isBitrate && isFps && !isResolution && CompressOption.FpsVsBitrateOption.TryGetValue(key, out string? fpsVsBitrateCommand))
				return fpsVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			// Nén resolution và bitrate
			if (isBitrate && !isFps && isResolution && CompressOption.ResolutionVsBitrateOption.TryGetValue(key, out string? resolutionVsBitrateCommand))
				return resolutionVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath);

			if (width < height)
				return $"-i {videoPath} -c:v libvpx-vp9 -vf pad=width=ih*16/9:height=ih:x=(ow-iw)/2:y=0 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {outputPath}";

			// Nén full option (nếu có) hoặc nén mặc định nếu không có optiin nào được chọn
			return (CompressOption.CompressFullOption.TryGetValue(key, out string? fullCommand))
				? fullCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", outputPath)
				: $"-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -af volume=0.5 -b:a 64K {outputPath}";
		}

		/// <summary>
		/// Lấy thông tin video
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fps"></param>
		/// <param name="bitrate"></param>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="Exception"></exception>
		public void GetVideoInfo(string videoPath, out int width, out int height, out int fps, out int bitrate)
		{
			if (!File.Exists(videoPath))
			{
				throw new VideoFileNotFoundException(videoPath);
			}

			string argument = $"-v error -select_streams v:0 -show_entries stream=width,height,r_frame_rate,bit_rate -of csv=p=0 {videoPath}";
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = "ffprobe",
				Arguments = argument,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			using (Process process = new Process())
			{
				process.StartInfo = psi;
				process.Start();

				string output = process.StandardOutput.ReadToEnd();
				string error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (!string.IsNullOrEmpty(error))
				{
					throw new FFprobeException(error);
				}
				if (string.IsNullOrEmpty(output))
				{
					throw new VideoInfoException("No output from ffprobe");
				}

				string[] lines = output.Split(',');

				if (lines.Length >= 4)
				{
					width = int.Parse(lines[0]);
					height = int.Parse(lines[1]);
					fps = ParseFps(lines[2]);
					bitrate = int.Parse(lines[3]) / 1024; // Đổi byte sang kb
				}
				else
				{
					throw new UnexpectedOutputFormatException();
				}
			}
		}

		/// <summary>
		/// Kiểm tra hợp lệ file đầu ra
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		public bool IsInvalidOutput(string output) => output == "notfound" || output == "cannotGetInfo";

		/// <summary>
		/// Cập nhật status của blob
		/// </summary>
		/// <param name="blob"></param>
		/// <returns></returns>
		public async Task UpdateStatus(Blob blob)
		{
			_context.blobs.Update(blob);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Lưu blob mới sau khi nén
		/// </summary>
		/// <param name="output"></param>
		/// <param name="blobDto"></param>
		/// <param name="oldBlob"></param>
		/// <param name="elapsedTime"></param>
		/// <returns></returns>
		public async Task SaveCompressBlobAsync(string output, BlobDto blobDto, Blob oldBlob, string elapsedTime)
		{
			byte[] fileBytes = await File.ReadAllBytesAsync(output);

			var metadata = new BsonDocument
				{
					{"filename", blobDto.BlobName },
					{"contentType", blobDto.ContentType },
					{"length",fileBytes.Length},
					{"uploadDate", oldBlob.UploadDate }
				};

			using (Stream stream = new MemoryStream(fileBytes))
			{
				var fileId = await _gridFSBucket.UploadFromStreamAsync(blobDto.BlobName, stream, new GridFSUploadOptions
				{
					Metadata = metadata
				});

				Blob newBlob = new Blob
				{
					BlobId = fileId.ToString(),
					ContainerId = blobDto.ContainerId,
					BlobName = blobDto.BlobName!,
					Status = "Compressed",
					ContentType = blobDto.ContentType!,
					Size = fileBytes.Length,
					CompressionTime = elapsedTime,
					UploadDate = oldBlob.UploadDate
				};
				await _context.blobs.AddAsync(newBlob);
				await _context.SaveChangesAsync();
			}
		}

		/// <summary>
		/// Định dạng lại fps
		/// </summary>
		/// <param name="fpsString"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		private int ParseFps(string fpsString)
		{
			var parts = fpsString.Split('/');
			if (parts.Length == 2 && int.TryParse(parts[0], out int numerator) && int.TryParse(parts[1], out int denominator))
			{
				return numerator / denominator;
			}

			throw new Exception("Invalid FPS format.");
		}
	}
}