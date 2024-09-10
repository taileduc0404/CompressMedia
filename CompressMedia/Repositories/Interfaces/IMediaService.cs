using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		//Task<ICollection<Media>> GetAllImage();
		Task<ICollection<Media>> GetAllVideo();
        bool UploadMedia(MediaDto mediaDto);
        //Task<string?> OptimizeImage(string imgPath, string fileName, string savePath);
        string OptimizeVideo(string videoPath, string fileNameOutput);
    }
}
