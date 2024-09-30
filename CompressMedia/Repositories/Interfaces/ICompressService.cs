using CompressMedia.DTOs;

namespace CompressMedia.Repositories.Interfaces
{
	public interface ICompressService
	{
		Task<string> CompressMedia(BlobDto blobDto);
	}
}
