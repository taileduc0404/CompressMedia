using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobContainerService
	{
		Task<bool> SaveAsync(ContainerDto containerDto);
		Task<bool> DeleteAsync(int containerId);
		//Task<bool> ExistsAsync(string name); 
		Task<ICollection<BlobContainer>> GetAsync();
		//Task<Stream?> GetOrNullAsync(string name);
	}
}
