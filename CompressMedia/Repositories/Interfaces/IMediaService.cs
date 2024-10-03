using CompressMedia.DTOs;
using CompressMedia.Enums;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		/// <summary>
		/// Tải file về
		/// </summary>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		Task<string> DownloadFile(BlobDto blobDto);

		/// <summary>
		/// Đổi định dạng file
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		string ChangeFileExtension(string extension);

		/// <summary>
		/// Cập nhật trạng thái
		/// </summary>
		/// <param name="blob"></param>
		/// <returns></returns>
		Task UpdateStatus(Blob blob);

		/// <summary>
		/// Kiểm tra hợp lệ file đầu ra
		/// </summary>
		/// <param name="output"></param>
		/// <returns></returns>
		bool IsInvalidOutput(string output);

		/// <summary>
		/// Lưu file mới sau khi xử lý với file đầu ra là string
		/// </summary>
		/// <param name="output"></param>
		/// <param name="blobDto"></param>
		/// <param name="oldBlob"></param>
		/// <param name="elapsedTime"></param>
		/// <returns></returns>
		Task SaveCompressBlobAsync(string output, BlobDto blobDto, Blob oldBlob, string elapsedTime);

		/// <summary>
		/// Lưu file mới sau khi xử lý với file đầu ra là byte[]
		/// </summary>
		/// <param name="output"></param>
		/// <param name="blobDto"></param>
		/// <param name="oldBlob"></param>
		/// <param name="elapsedTime"></param>
		/// <returns></returns>
		Task SaveCompressBlobAsync(byte[] output, BlobDto blobDto, Blob oldBlob, string elapsedTime);

		/// <summary>
		/// Lưu file mới sau khi xử lý với file đầu ra là byte[]
		/// </summary>
		/// <param name="output"></param>
		/// <param name="blobDto"></param>
		/// <param name="oldBlob"></param>
		/// <param name="elapsedTime"></param>
		/// <returns></returns>
		Task SaveCompressBlobAsync(Stream output, BlobDto blobDto, Blob oldBlob, string elapsedTime);

		/// <summary>
		/// Xóa blob cũ
		/// </summary>
		/// <param name="oldBlob"></param>
		/// <param name="blobDto"></param>
		/// <returns></returns>
		Task DeleteOldBlob(Blob oldBlob, BlobDto blobDto);

		/// <summary>
		/// Thực thi command
		/// </summary>
		/// <param name="arguments"></param>
		void ExecuteCommand(string arguments);

		/// <summary>
		/// Lấy thông tin video
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fps"></param>
		/// <param name="bitrate"></param>
		void GetVideoInfo(string videoPath, out int width, out int height, out int fps, out int bitrate);

		/// <summary>
		/// Xóa các file tạm
		/// </summary>
		void DeleteTemporaryFiles();

		/// <summary>
		/// Mã xử lý video
		/// </summary>
		/// <param name="videoPath"></param>
		/// <param name="outputPath"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fps"></param>
		/// <param name="isFps"></param>
		/// <param name="isResolution"></param>
		/// <param name="isBitrate"></param>
		/// <returns></returns>
		string GenerateVideoCompressionCommand(string videoPath, string outputPath, int width, int height, string fps, bool isFps, bool isResolution, bool isBitrate);

		/// <summary>
		/// Mã xử lý hình ảnh
		/// </summary>
		/// <param name="imageInput"></param>
		/// <param name="outputPath"></param>
		/// <param name="imageManipulationMode"></param>
		/// <param name="imageZoomMode"></param>
		/// <param name="aspectRatioMode"></param>
		/// <returns></returns>
		string GenerateImageCompressionCommand(string imageInput, string outputPath, ImageManipulationMode? imageManipulationMode, ImageZoomMode? imageZoomMode, ImageAspectRatioMode? aspectRatioMode);

	}
}
