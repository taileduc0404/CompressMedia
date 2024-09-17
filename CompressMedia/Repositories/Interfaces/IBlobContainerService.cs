using CompressMedia.DTOs;
using CompressMedia.Models;
using System.ComponentModel;

namespace CompressMedia.Repositories.Interfaces
{
	public interface IBlobContainerService
	{
		Task<bool> SaveAsync(ContainerDto containerDto);
		Task<bool> DeleteAsync(string name);
		//Task<bool> ExistsAsync(string name); 
		Task<ICollection<BlobContainer>> GetAsync();
		//Task<Stream?> GetOrNullAsync(string name);
	}
}
