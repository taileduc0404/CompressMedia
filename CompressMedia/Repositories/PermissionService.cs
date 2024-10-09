using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public PermissionService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public string CreatePermission(PermissionDto permissionDto)
        {
            if (permissionDto is null)
            {
                return null!;
            }

            Permission permission = new Permission()
            {
                PermissionId = permissionDto.PermissionId,
                PermissionName = permissionDto.PermissionName,
                PermissionDescription = permissionDto.PermissionDescription
            };
            _context.Add(permission);
            _context.SaveChanges();
            return "ok";
        }

        public string DeletePermission(int permissionId)
        {
            Permission? permission = _context.Permissions.SingleOrDefault(x => x.PermissionId == permissionId);
            if (permission is null)
            {
                return null!;
            }
            _context.Remove(permissionId);
            _context.SaveChanges();
            return "ok";
        }

        public IEnumerable<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList() ?? null!;
        }

        public Permission GetPermissionById(int permissionId)
        {
            throw new NotImplementedException();
        }

        public Permission GetPermissionByName(string permissionName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RolePermission>> GetPermissionByRoleId(int roleId)
        {
            return roleId > 0 ? await _context.RolePermissions.Where(x => x.RoleId == roleId).ToListAsync() : null!;
        }

        public async Task<List<Permission>> GetUserPermissionsAsync()
        {

            string username = _userService.GetUserNameLoggedIn();
            User? userId = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            User? user = await _context.Users!
                .FirstOrDefaultAsync(u => u.UserId == userId!.UserId);

            if (user == null)
            {
                return new List<Permission>();
            }

            List<Permission> permissions = user.UserPermissions!
                                                    .Select(x => x.Permission)
                                                    .Distinct()
                                                    .ToList()!;
            return permissions!;
        }

        public bool PermissionExists(int permissionId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePermission(PermissionDto permissionDto)
        {
            throw new NotImplementedException();
        }
    }
}
