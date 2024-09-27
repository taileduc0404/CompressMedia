using CompressMedia.DTOs;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IMediaService
	{
		Task<string> DownloadFile(BlobDto blobDto);
		Task<string> CompressMedia(BlobDto blobDto);
		Task<string> OptimizeVideo(BlobDto blobDto);

	}
}
