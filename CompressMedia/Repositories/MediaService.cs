﻿using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
	public class MediaService : IMediaService
	{
		private readonly ApplicationDbContext _context;
		private readonly IAuthService _authService;
		private readonly IFileProvider _fileProvider;
		public MediaService(ApplicationDbContext context, IAuthService authService, IFileProvider fileProvider)
		{
			_context = context;
			_authService = authService;
			_fileProvider = fileProvider;
		}

		/// <summary>
		/// Tìm media bằng Id
		/// </summary>
		/// <param name="mediaId"></param>
		/// <returns></returns>
		public async Task<Media> GetMediaById(int mediaId)
		{
			Media? media = await _context.medias.SingleOrDefaultAsync(m => m.MediaId == mediaId);
			if (media == null)
			{
				return null!;
			}

			return media;
		}

		/// <summary>
		/// Lấy ra danh sách video của user đagn đăngg nhập
		/// </summary>
		/// <returns></returns>
		public async Task<ICollection<Media>> GetAllVideo()
		{
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = await _context.users.FirstOrDefaultAsync(u => u.Username == userInfo.Username);

			return await _context.medias.Where(m => m.MediaType!.StartsWith("video") && m.UserId == user!.UserId).ToListAsync();
		}

		/// <summary>
		/// Thực thi command
		/// </summary>
		/// <param name="arguments"></param>
		private void ExecuteCommand(string arguments)
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
		/// Lấy thông tin video
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fps"></param>
		/// <param name="bitrate"></param>
		/// <exception cref="FileNotFoundException"></exception>
		/// <exception cref="Exception"></exception>
		private void GetVideoInfo(string videoPath, out int width, out int height, out int fps, out int bitrate)
		{
			if (!File.Exists(videoPath))
			{
				throw new FileNotFoundException($"The file {videoPath} does not exist");
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
					throw new Exception($"FFprobe Error: {error}");
				}
				if (string.IsNullOrEmpty(output))
				{
					throw new Exception("No output from ffprobe.");
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
					throw new Exception("Unexpected ffprobe output format.");
				}
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

		/// <summary>
		/// Chọn option: nén video.
		/// </summary>
		/// <param name="options"></param>
		/// <param name="videoPath"></param>
		/// <param name="fileNameOutput"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>  
		private static string GetOption(string videoPath, string outputPath, int width, int height, string fps, bool isFps, bool isResolution, bool isBitrate)
		{
			string root = "/Medias/Videos/";
			string resolution = $"{width}x{height}";
			string key = $"{resolution}_{fps}_{(isFps ? "true" : "false")}Fps_{(isResolution ? "true" : "false")}Resolution_{(isBitrate ? "true" : "false")}Bitrate";

			// Trường hợp chỉ nén fps
			if (isFps && !isResolution && !isBitrate && CompressOption.FpsOnlyOption.TryGetValue(key, out string? fpsCommand))
			{
				return fpsCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Trường hợp chỉ nén resolution
			if (isResolution && !isFps && !isBitrate && CompressOption.ResolutionOnlyOption.TryGetValue(key, out string? resolutionCommand))
			{
				return resolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Trwufogn hợp chỉ nén bitrate
			if (isBitrate && !isFps && !isResolution && CompressOption.BitrateOnlyOption.TryGetValue(key, out string? bitrateCommand))
			{
				return bitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Trường hợp nén fps và resolution
			if (!isBitrate && isFps && isResolution && CompressOption.FpsVsResolutionOption.TryGetValue(key, out string? fpsVsResolutionCommand))
			{
				return fpsVsResolutionCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Trường hợp nén fps và bitrate
			if (isBitrate && isFps && !isResolution && CompressOption.FpsVsBitrateOption.TryGetValue(key, out string? fpsVsBitrateCommand))
			{
				return fpsVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Trường hợp nén resolution và bitrate
			if (isBitrate && !isFps && isResolution && CompressOption.ResolutionVsBitrateOption.TryGetValue(key, out string? resolutionVsBitrateCommand))
			{
				return resolutionVsBitrateCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			if (width < height)
			{
				return $"-i {videoPath} -c:v libvpx-vp9 -vf pad=width=ih*16/9:height=ih:x=(ow-iw)/2:y=0 -preset ultrafast -b:v 1M -minrate 500K -maxrate 964K -bufsize 2M -crf 30 -af volume=0.5 -b:a 64K {"wwwroot" + root + outputPath}";
			}

			// Trường hợp nén full option
			if (CompressOption.CompressFullOption.TryGetValue(key, out string? fullCommand))
			{
				return fullCommand.Replace("{videoPath}", videoPath).Replace("{outputPath}", "wwwroot" + root + outputPath);
			}

			// Truonwggf hợp người dùng không chọn option nào
			return $"-i {videoPath} -c:v libvpx-vp9 -preset ultrafast -af volume=0.5 -b:a 64K {"wwwroot" + root + outputPath}";
		}

		/// <summary>
		/// Nén video
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="fileNameOutput"></param>
		/// <returns></returns>
		public string OptimizeVideo(string videoPath, string fileNameOutput, MediaDto mediaDto)
		{
			if (videoPath is not null)
			{
				GetVideoInfo(videoPath, out int width, out int height, out int fps, out int bitrate);

				string fpsString = $"{fps}fps";
				string arg = GetOption(videoPath, fileNameOutput, width, height, fpsString, mediaDto.IsFps, mediaDto.IsResolution, mediaDto.IsBitrateVideo);

				ExecuteCommand(arg);
			}
			return fileNameOutput;
		}

		/// <summary>
		/// Nén media
		/// </summary>
		/// <param name="mediaDto"></param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public bool CompressMedia(string fileNameInput, MediaDto mediaDto)
		{
			// Kiểm tra người dùng có đăng nhập chưa
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = _context.users.FirstOrDefault(u => u.Username == userInfo.Username);

			string root = "/Medias/Videos/";
			// fileNameInput: 7a87788f-f8a7-45b1-83b4-b2e2eb9bc30b&test.mp4

			string rootPathFileNameInput = root + fileNameInput;
			IFileInfo fileInputInfo = _fileProvider.GetFileInfo(rootPathFileNameInput);
			string? rootPathFileInputInfo = fileInputInfo.PhysicalPath;
			Media deleteMedia = _context.medias.FirstOrDefault(m => m.MediaPath == rootPathFileInputInfo)!;

			deleteMedia.Status = "Compressing...";
			_context.Update(deleteMedia);
			_context.SaveChanges();

			string fileNameOutput = Guid.NewGuid() + "&" + fileNameInput!.Split('&')[1];

			// Bộ đếm thời gian nensn vidoe
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			string result = OptimizeVideo("wwwroot" + rootPathFileNameInput, fileNameOutput, mediaDto);
			stopwatch.Stop();

			TimeSpan timeSpan = stopwatch.Elapsed;

			string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

			string srcOptimized = root + result;
			IFileInfo fileInfoOptimized = _fileProvider.GetFileInfo(srcOptimized);
			string? rootPathOptimized = fileInfoOptimized.PhysicalPath!;
			if (rootPathOptimized != null)
			{
				// Xóa ảnh gốc sau khi optimize
				File.Delete("wwwroot" + rootPathFileNameInput);

				_context.medias.Remove(deleteMedia);

				if (!File.Exists(rootPathOptimized))
				{
					throw new FileNotFoundException("File optimized does not exist.", rootPathOptimized);
				}

				// Lấy độ dài file của ảnh đã optimize
				FileInfo fileInfo = new FileInfo(rootPathOptimized);
				long fileLength = fileInfo.Length;

				// Xác định loại media
				string mediaType = fileInfo.Extension.ToLower() switch
				{
					".mp4" => "video/mp4",
					".jpg" => "image/jpg",
					".jpeg" => "image/jpeg",
					".png" => "image/png",
					".gif" => "image/gif",
					_ => "unknown"
				};

				Media media = new Media
				{
					MediaPath = rootPathOptimized,
					CreatedDate = DateTime.Now,
					Size = fileLength,
					Status = "Compressed",
					CompressDuration = elapsedTime,
					MediaType = mediaType,
					UserId = user!.UserId
				};

				_context.medias.Add(media);
				_context.SaveChanges();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Upload media
		/// </summary>
		/// <param name="mediaDto"></param>
		/// <returns></returns>
		public string UploadMedia(MediaDto mediaDto)
		{
			// Kiểm tra người dùng có đăng nhập chưa
			string cookie = _authService.GetLoginInfoFromCookie();
			string cookieDecode = _authService.DecodeFromBase64(cookie);
			LoginDto userInfo = JsonConvert.DeserializeObject<LoginDto>(cookieDecode);
			User? user = _context.users.FirstOrDefault(u => u.Username == userInfo.Username);

			// Kiểm tra xem người dùng có upload file media lên chưa
			if (mediaDto.Media != null)
			{
				string root = "/Medias/Videos/";
				string imageName = $"{Guid.NewGuid() + "&" + mediaDto.Media.FileName}";
				//string imageName = $"{mediaDto.Media.FileName}";

				if (!Directory.Exists("wwwroot" + root))
				{
					Directory.CreateDirectory("wwwroot" + root);
				}

				string src = root + imageName;
				IFileInfo imgInfo = _fileProvider.GetFileInfo(src);
				string? rootPath = imgInfo.PhysicalPath!;

				if (string.IsNullOrEmpty(rootPath)) return null!;
				using (var fileStream = new FileStream(rootPath, FileMode.Create))
				{
					mediaDto.Media.CopyTo(fileStream);
				}

				if (string.IsNullOrEmpty(rootPath)) return null!;

				if (rootPath != null)
				{
					// Lấy độ dài file của ảnh đã optimize
					FileInfo fileInfo = new FileInfo(rootPath);
					long fileLength = fileInfo.Length;

					// Xác định loại media
					string mediaType = fileInfo.Extension.ToLower() switch
					{
						".mp4" => "video/mp4",
						".jpg" => "image/jpg",
						".jpeg" => "image/jpeg",
						".png" => "image/png",
						".gif" => "image/gif",
						_ => "unknown"
					};

					Media media = new Media()
					{
						MediaPath = rootPath,
						CreatedDate = DateTime.Now,
						Size = fileLength,
						Status = "Original",
						MediaType = mediaType,
						UserId = user!.UserId
					};

					_context.medias.Add(media);
					_context.SaveChanges();
					return "wwwroot" + src;
				}
				return null!;
			}
			return null!;
		}

		/// <summary>
		/// Xóa video
		/// </summary>
		/// <param name="mediaId"></param>
		/// <returns></returns>
		public async Task<bool> DeleteMedia(int mediaId)
		{
			string splitString = @"D:\BÀI TẬP\ASP.NET\CompressMedia\CompressMedia\";
			Media? media = await _context.medias.SingleOrDefaultAsync(m => m.MediaId == mediaId);
			if (media == null) return false;
			string mediaPath = media.MediaPath!.Replace(splitString, "");
			File.Delete(mediaPath);
			_context.medias.Remove(media);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}