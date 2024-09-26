using CompressMedia.DTOs;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		//Task<ICollection<Media>> GetAllVideo();
		//Task<Media> GetMediaById(int mediaId);
		//Task<bool> DeleteMedia(int mediaId);
		//string UploadMedia(MediaDto mediaDto);
		//bool CompressMedia(string fileNameInput, MediaDto mediaDto);
		//string OptimizeVideo(string videoPath, string fileNameOutput, MediaDto mediaDto);
		Task<string> DownloadFile(BlobDto blobDto);
		Task<bool> CompressMedia(BlobDto blobDto);
		Task<string> OptimizeVideo(BlobDto blobDto);

	}
}
