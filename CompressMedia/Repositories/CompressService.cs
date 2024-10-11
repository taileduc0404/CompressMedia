using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Exceptions;
using CompressMedia.Imaging;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.GridFS;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
	public class CompressService : ICompressService
	{
		private readonly IMediaService _mediaService;
		private readonly ApplicationDbContext _context;
		public readonly BlobStorageDbContext _storageContext;
		private readonly IImageResizer _imageResizer;
		public readonly IGridFSBucket _gridFSBucket;

		public CompressService(IMediaService mediaService, ApplicationDbContext context, BlobStorageDbContext storageContext, IImageResizer imageResizer)
		{
			_mediaService = mediaService;
			_context = context;
			_storageContext = storageContext ?? throw new ArgumentNullException(nameof(storageContext));
			_gridFSBucket = new GridFSBucket(_storageContext._mongoDatabase);
			_imageResizer = imageResizer;
		}

		/// <summary>
		/// Resize hoặc nén media dùng ffmpeg
		/// </summary>
		/// <param name="mediaDto"></param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public async Task<string> CompressMedia(BlobDto blobDto)
		{
			Blob? oldBlob = await _context.Blobs.FirstOrDefaultAsync(b => b.BlobId == blobDto.BlobId);
			if (oldBlob is null) return "notfound";

			oldBlob!.Status = "Compressing...";
			await _mediaService.UpdateStatus(oldBlob);

			Stopwatch stopwatch = Stopwatch.StartNew();
			string fileTempOutput = await OptimizeMedia(blobDto);
			stopwatch.Stop();

			if (_mediaService.IsInvalidOutput(fileTempOutput)) return fileTempOutput;

			string elapsedTime = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

			await _mediaService.SaveCompressBlobAsync(fileTempOutput, blobDto, oldBlob, elapsedTime);
			await _mediaService.DeleteOldBlob(oldBlob, blobDto);

			_mediaService.DeleteTemporaryFiles();
			return "ok";
		}

		/// <summary>
		/// Resize hoặc nén media dùng MagickNET
		/// </summary>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		public async Task<string> CompressMediaWithMagickNET(BlobDto blobDto)
		{
			Blob? oldBlob = await _context.Blobs.FirstOrDefaultAsync(b => b.BlobId == blobDto.BlobId);
			if (oldBlob is null) return "notfound";

			oldBlob!.Status = "Resizing...";
			await _mediaService.UpdateStatus(oldBlob);

			string blob = await _mediaService.DownloadFile(blobDto);

			using (MagickImage image = new MagickImage(blob))
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				image.Density = new Density(160);
				MagickGeometry size = new MagickGeometry(320, 480)
				{
					IgnoreAspectRatio = false
				};

				image.Resize(size);

				using MemoryStream mStream = new MemoryStream();
				image.Write(mStream, MagickFormat.Jpg);

				byte[] resizedImageBytes = mStream.ToArray();
				stopwatch.Stop();

				string elapsedTime = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

				await _mediaService.SaveCompressBlobAsync(resizedImageBytes, blobDto, oldBlob, elapsedTime);

				await _mediaService.DeleteOldBlob(oldBlob, blobDto);
				_mediaService.DeleteTemporaryFiles();
			}
			return "ok";
		}

		/// <summary>
		/// Đổi kích thước hình ảnh (có thể tùy chọn service)
		/// </summary>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		public async Task<string> ImageResizer(BlobDto blobDto)
		{
			Blob? oldBlob = await _context.Blobs.FirstOrDefaultAsync(b => b.BlobId == blobDto.BlobId);
			if (oldBlob is null) return "notfound";

			oldBlob!.Status = "Resizing...";
			await _mediaService.UpdateStatus(oldBlob);

			string blob = await _mediaService.DownloadFile(blobDto);
			using (FileStream imageStream = new FileStream(blob, FileMode.Open, FileAccess.ReadWrite))
			{
				imageStream.Position = 0;
				Stopwatch stopwatch = Stopwatch.StartNew();
				var resizeResult = await _imageResizer.ResizeAsync(
					imageStream,
					new ImageResizeArgs
					{
						Width = blobDto.Width,
						Height = blobDto.Height,
						Mode = ImageResizeMode.Max
					},
					mimeType: $"{blobDto.ContentType}"
				);
				Stream? resultStream = imageStream;
				if (resizeResult.Result is not null && resizeResult.State == ImageProcessState.Done && resizeResult.Result.CanRead)
				{
					resultStream = resizeResult.Result!;
				}
				stopwatch.Stop();

				string elapsedTime = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

				await _mediaService.SaveCompressBlobAsync(resultStream, blobDto, oldBlob, elapsedTime);
				await _mediaService.DeleteOldBlob(oldBlob, blobDto);
				return "ok";
			}
		}

		/// <summary>
		/// Xử lý nén
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="fileNameOutput"></param>
		/// <returns></returns>
		private async Task<string> OptimizeMedia(BlobDto blobDto)
		{
			string tempFile = await _mediaService.DownloadFile(blobDto);
			if (tempFile == null)
			{
				return "fileDownloadFailed";
			}

			string fileTempOuputFinal = _mediaService.ChangeFileExtension(blobDto.ContentType == "video/mp4" ? ".mp4" : ".jpg");
			string arg = string.Empty;

			if (tempFile != null)
			{
				try
				{
					if (blobDto.ContentType == "video/mp4")
					{
						_mediaService.GetVideoInfo(tempFile, out int width, out int height, out int fps, out int bitrate);
						string fpsString = $"{fps}fps";
						arg = _mediaService.GenerateVideoCompressionCommand(tempFile, fileTempOuputFinal, width, height, fpsString, blobDto.IsFps, blobDto.IsResolution, blobDto.IsBitrateVideo);
					}
					else if (blobDto.ContentType == "image/jpeg")
					{
						arg = _mediaService.GenerateImageCompressionCommand(tempFile, fileTempOuputFinal, blobDto.ImageResizeMode, blobDto.ImageZoomMode, blobDto.ImageAspectRatioMode);
					}

					if (!string.IsNullOrEmpty(arg))
					{
						_mediaService.ExecuteCommand(arg);
					}
				}
				catch (VideoFileNotFoundException)
				{
					return "notfound";
				}
				catch (FFprobeException)
				{
					return "cannotGetInfo";
				}
				catch (UnexpectedOutputFormatException)
				{
					return "cannotGetInfo";
				}
			}
			return fileTempOuputFinal;
		}
	}
}
