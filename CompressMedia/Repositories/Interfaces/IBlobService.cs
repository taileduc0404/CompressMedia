using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobService
	{
		Task<bool> CreateBlobAsync(BlobDto blobDto);
		Task<bool> DeleteBlobAsync(string blobId);
		Task<bool> GetBlobContentAsync(BlobDto blobDto);
		Task GetBlobMetadataAsync();
		Task<ICollection<Blob>> GetListBlobAsync(int containerId);
		Task UpdateBlobAsync();
		Task<bool> CompressMedia(BlobDto blobDto);
		Task<string> OptimizeVideo(BlobDto blobDto);
		Task<Stream> GetBlobStreamAsync(string blobId);
	}
}
