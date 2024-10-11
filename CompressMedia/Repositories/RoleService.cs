using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
	public class RoleService : IRoleService
	{
		private readonly ApplicationDbContext _context;
		private readonly IUserService _userService;

		public RoleService(ApplicationDbContext context, IUserService userService)
		{
			_context = context;
			_userService = userService;
		}

		/// <summary>
		/// Gán quyền vào role
		/// </summary>
		/// <param name="roleId"></param>
		/// <param name="permissionSelectedId"></param>
		/// <returns></returns>
		public async Task<string> AssignPermissionsToRole(int roleId, List<int> permissionSelectedId)
		{
			var role = _context.Roles.Include(r => r.RolePermissions).FirstOrDefault(r => r.RoleId == roleId);
			if (role == null)
			{
				return "Role not found.";
			}

			foreach (var permissionId in permissionSelectedId)
			{
				var permission = _context.Permissions.Find(permissionId);
				if (permission != null)
				{
					role.RolePermissions!.Add(new RolePermission
					{
						RoleId = roleId,
						PermissionId = permissionId
					});
				}

			}

			List<User> users = _context.Users.Include(x => x.UserPermissions).Where(x => x.RoleId == roleId).ToList();
			foreach (User user in users)
			{
				foreach (int permissionId in permissionSelectedId)
				{
					if (!user.UserPermissions!.Any(x => x.PermissionId == permissionId))
					{
						user.UserPermissions!.Add(new UserPermission
						{
							UserId = user.UserId,
							PermissionId = permissionId
						});
					}
				}
			}

			await _context.SaveChangesAsync();
			return "Permissions updated.";
		}

		/// <summary>
		/// Tạo role
		/// </summary>
		/// <param name="roleDto"></param>
		/// <param name="tenantId"></param>
		/// <returns></returns>
		public string CreateRole(RoleDto roleDto, Guid? tenantId)
		{
			if (roleDto is not null)
			{
				Role role = new Role()
				{
					RoleName = roleDto.RoleName,
					TenantId = tenantId,
				};
				_context.Add(role);
				_context.SaveChanges();
				return "ok";
			}
			return null!;
		}

		/// <summary>
		/// Xóa role
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		public string DeleteRole(int roleId)
		{
			if (roleId > 0)
			{
				List<RolePermission> permissions = _context.RolePermissions.Where(r => r.RoleId == roleId).ToList();

				foreach (var permission in permissions)
				{
					_context.Remove(permission);
				}

				_context.Roles.Remove(_context.Roles.FirstOrDefault(x => x.RoleId == roleId)!);
				_context.SaveChanges();
				return "ok";
			}
			return null!;
		}

		/// <summary>
		/// Lấy danh sách role
		/// </summary>
		/// <param name="tenantId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<Role>> GetAllRoles(Guid? tenantId)
		{
			string username = _userService.GetUserNameLoggedIn();
			User? user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

			IEnumerable<Role> roles;
			if (user!.TenantId is not null)
			{
				roles = _context.Roles.Where(x => x.TenantId == tenantId).ToList();
			}
			else
			{
				roles = _context.Roles.ToList();
			}
			return roles.Any() is true ? roles : null!;
		}

		/// <summary>
		/// Lấy role bằng id
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		public Role GetRoleById(int roleId)
		{
			return _context.Roles.SingleOrDefault(x => x.RoleId == roleId) ?? null!;
		}

		/// <summary>
		/// Lấy danh sách permission của 1 role
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<Permission>> GetRolePermission(int roleId)
		{
			var permissions = await _context.RolePermissions
											.Where(x => x.RoleId == roleId)
											.Select(x => x.Permission)
											.ToListAsync();

			return permissions.Any() ? permissions! : Enumerable.Empty<Permission>();
		}

	}
}
