using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IRoleService
    {
        string CreateRole(RoleDto roleDto, Guid? tenantId);
        void UpdateRole(RoleDto roleDto);
        string DeleteRole(int roleId);
        Task<IEnumerable<Role>> GetAllRoles(Guid? tenantId);
        Role GetRoleById(int roleId);
        bool RoleExists(int roleId);
        Role GetRoleByName(string roleName);
        Task<IEnumerable<Permission>> GetRolePermission(int userId);
        Task<string> AssignPermissionsToRole(int roleId, List<int> permissionSelectedId);
    }
}
