
using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
	public interface ITenantService
	{
		Task<string> CreateTenantAsync(TenantDto tenantDto);
		Task<bool> DeleteAsync(int tenantId);
		Task<IEnumerable<Tenant>> GetAllAsync();
		Task<string> AddUser(RegisterDto dto, Guid? tenantId);
	}
}
