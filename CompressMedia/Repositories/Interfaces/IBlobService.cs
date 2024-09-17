using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobService
	{
		Task<bool> CreateBlobAsync(BlobDto blobDto);
		Task DeleteBlobAsync();
		Task GetBlobContentAsync();
		Task GetBlobMetadataAsync();
		Task<ICollection<Blob>> GetListBlobAsync(int containerId);
		Task UpdateBlobAsync();
	}
}
