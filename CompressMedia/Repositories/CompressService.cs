using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Exceptions;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CompressMedia.Repositories
{
	public class CompressService : ICompressService
	{
		private readonly IMediaService _mediaService;
		private readonly ApplicationDbContext _context;

		public CompressService(IMediaService mediaService, ApplicationDbContext context)
		{
			_mediaService = mediaService;
			_context = context;
		}
		/// <summary>
		/// Nén media
		/// </summary>
		/// <param name="mediaDto"></param>
		/// <returns></returns>
		/// <exception cref="FileNotFoundException"></exception>
		public async Task<string> CompressMedia(BlobDto blobDto)
		{
			Blob? oldBlob = await _context.blobs.FirstOrDefaultAsync(b => b.BlobId == blobDto.BlobId);
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
