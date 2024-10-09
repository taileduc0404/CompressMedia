using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IPermissionService
    {
        string CreatePermission(PermissionDto PermissionDto);
        void UpdatePermission(PermissionDto PermissionDto);
        string DeletePermission(int PermissionId);
        IEnumerable<Permission> GetAllPermissions();
        Permission GetPermissionById(int PermissionId);
        bool PermissionExists(int PermissionId);
        Permission GetPermissionByName(string PermissionName);
        Task<List<Permission>> GetUserPermissionsAsync();
        Task<List<RolePermission>> GetPermissionByRoleId(int roleId);
    }
}
