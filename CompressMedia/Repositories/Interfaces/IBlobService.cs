using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobService
	{
		Task<bool> CreateBlobAsync(BlobDto blobDto);
		Task<string> DeleteBlobAsync(string blobId);
		Task<bool> GetBlobContentAsync(BlobDto blobDto);
		Task<Stream> GetBlobStreamAsync(string blobId);
		Task GetBlobMetadataAsync();
		Task<ICollection<Blob>> GetListBlobAsync(int containerId);
		Task UpdateBlobAsync();

	}
}
