using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IPermissionService
    {
        IEnumerable<Permission> GetAllPermissions();
        Task<List<Permission>> GetUserPermissionsAsync();
        Task<List<RolePermission>> GetPermissionByRoleId(int roleId);
    }
}
