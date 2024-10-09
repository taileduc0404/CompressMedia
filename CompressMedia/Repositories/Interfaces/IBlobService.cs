using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IBlobService
    {
        Task<bool> CreateBlobAsync(BlobDto blobDto);
        Task<string> DeleteBlobAsync(string blobId);
        Task<Stream> GetBlobStreamAsync(string blobId);
        Task<ICollection<Blob>> GetListBlobAsync(int containerId);
    }
}
