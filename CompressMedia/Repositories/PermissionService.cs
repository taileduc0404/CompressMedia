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

        /// <summary>
        /// Lấy danh sách permission
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Permission> GetAllPermissions()
        {
            return _context.Permissions.ToList() ?? null!;
        }

        /// <summary>
        /// Lấy danh sách permission bằng roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<RolePermission>> GetPermissionByRoleId(int roleId)
        {
            return roleId > 0 ? await _context.RolePermissions.Where(x => x.RoleId == roleId).ToListAsync() : null!;
        }

        /// <summary>
        /// Lấy danh sách permission của user đang đăng nhập
        /// </summary>
        /// <returns></returns>
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
    }
}
