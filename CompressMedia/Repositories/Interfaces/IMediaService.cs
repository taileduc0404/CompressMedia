using CompressMedia.DTOs;
using CompressMedia.Enums;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		Task<string> DownloadFile(BlobDto blobDto);
		string ChangeFileExtension(string extension);
		Task UpdateStatus(Blob blob);
		bool IsInvalidOutput(string output);
		Task SaveCompressBlobAsync(string output, BlobDto blobDto, Blob oldBlob, string elapsedTime);
		Task DeleteOldBlob(Blob oldBlob, BlobDto blobDto);
		void ExecuteCommand(string arguments);
		void GetVideoInfo(string videoPath, out int width, out int height, out int fps, out int bitrate);
		void DeleteTemporaryFiles();
		string GenerateVideoCompressionCommand(string videoPath, string outputPath, int width, int height, string fps, bool isFps, bool isResolution, bool isBitrate);
		string GenerateImageCompressionCommand(string imageInput, string outputPath, ImageManipulationMode? imageManipulationMode, ImageZoomMode? imageZoomMode, ImageAspectRatioMode? aspectRatioMode);
	}
}
