using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		Task<ICollection<Media>> GetAllVideo();
		Task<Media> GetMediaById(int mediaId);
        string UploadMedia(MediaDto mediaDto);
		bool CompressMedia(string fileNameInput);
		string OptimizeVideo(string videoPath, string fileNameOutput);
	}
}
