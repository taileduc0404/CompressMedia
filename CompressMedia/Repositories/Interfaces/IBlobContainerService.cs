using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobContainerService
	{
		Task<string> SaveAsync(ContainerDto containerDto);
		Task<bool> DeleteAsync(int containerId);
		Task<ICollection<BlobContainer>> GetAsync();
		Task<ICollection<BlobContainer>> GetAsync(Guid? tenantId);
	}
}
