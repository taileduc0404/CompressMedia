using CompressMedia.CustomAuthentication;
using CompressMedia.Data;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Repositories
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPermissionService _permissionService;

        public TenantService(ApplicationDbContext context, IAuthService authService, IPermissionService permissionService)
        {
            _context = context;
            _authService = authService;
            _permissionService = permissionService;
        }

        /// <summary>
        /// Tạo tenant
        /// </summary>
        /// <param name="tenantDto"></param>
        /// <returns></returns>
        public async Task<string> CreateTenantAsync(TenantDto tenantDto)
        {
            if (tenantDto == null)
            {
                return null!;
            }

            Tenant tenant = new Tenant
            {
                TenantId = tenantDto.TenantId,
                TenantName = tenantDto.TenantName,
                CompanyName = tenantDto.CompanyName,
                Users = new List<User>
                {
                    new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        TenantId = tenantDto.TenantId,
                        Username = tenantDto.RegisterDto?.Username,
                        FirstName = tenantDto.RegisterDto?.FirstName,
                        LastName = tenantDto.RegisterDto?.LastName,
                        SecretKey = Guid.NewGuid().ToString(),
                        Email = tenantDto.RegisterDto?.Email,
                        PasswordHash = PasswordHasher.Hash(tenantDto.RegisterDto?.Password ?? string.Empty),
                        UserPermissions = new List<UserPermission>()
                    }
                },
                Roles = new List<Role>()
            };

            if (!tenant.Roles.Any(r => r.RoleName == "Admin"))
            {
                var adminRole = new Role
                {
                    RoleName = "Admin",
                    TenantId = tenantDto.TenantId
                };

                // Thêm vai trò vào danh sách
                tenant.Roles.Add(adminRole);

                // Lưu tenant trước để EF biết RoleId
                await _context.Tenants.AddAsync(tenant);
                await _context.SaveChangesAsync(); // Lưu tại đây để RoleId được tạo

                User user = tenant.Users.First();

                List<RolePermission> rolePermissions = new List<RolePermission>
                {
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=2},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=3},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=4},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=5},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=6},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=7},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=8},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=9},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=10},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=11},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=12},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=13},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=14},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=15},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=16},
                    new RolePermission{RoleId=adminRole.RoleId, PermissionId=17},
                };

                // Thêm RolePermissions vào danh sách
                foreach (var rolePermission in rolePermissions)
                {
                    await _context.RolePermissions.AddAsync(rolePermission);
                }

                List<UserPermission> userPermissions = new List<UserPermission>
                {
                    new UserPermission { UserId = user.UserId, PermissionId = 2 },
                    new UserPermission { UserId = user.UserId, PermissionId = 3 },
                    new UserPermission { UserId = user.UserId, PermissionId = 4 },
                    new UserPermission { UserId = user.UserId, PermissionId = 5 },
                    new UserPermission { UserId = user.UserId, PermissionId = 6 },
                    new UserPermission { UserId = user.UserId, PermissionId = 7 },
                    new UserPermission { UserId = user.UserId, PermissionId = 8 },
                    new UserPermission { UserId = user.UserId, PermissionId = 9 },
                    new UserPermission { UserId = user.UserId, PermissionId = 10 },
                    new UserPermission { UserId = user.UserId, PermissionId = 11 },
                    new UserPermission { UserId = user.UserId, PermissionId = 12 },
                    new UserPermission { UserId = user.UserId, PermissionId = 13 },
                    new UserPermission { UserId = user.UserId, PermissionId = 14 },
                    new UserPermission { UserId = user.UserId, PermissionId = 15 },
                };

                foreach (var item in userPermissions)
                {
                    user.UserPermissions!.Add(item);
                }

                await _context.SaveChangesAsync(); // Lưu lại các UserPermissions sau khi đã thêm
            }

            await _authService.SendQrCodeViaEmail(tenantDto.RegisterDto!);
            return "ok";
        }

        /// <summary>
        /// Xóa tenant
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeleteAsync(int tenantId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy danh sách Tenant
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync() ?? null!;
        }

        /// <summary>
        /// Thêm user vào tenant
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> AddUser(RegisterDto dto, Guid? tenantId)
        {
            try
            {
                if (dto is null)
                {
                    throw new ArgumentNullException(nameof(dto), "RegisterDto cannot be null");
                }

                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    throw new ArgumentException("Password cannot be null or whitespace", nameof(dto.Password));
                }

                if (_authService.CheckUsernameAndEmail(dto))
                {
                    return "usernameExist";
                }

                List<RolePermission> permissions = await _permissionService.GetPermissionByRoleId(dto.SelectedRoleId);


                var user = new User
                {
                    UserId = Guid.NewGuid().ToString(),
                    TenantId = tenantId,
                    Username = dto.Username,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = PasswordHasher.Hash(dto.Password).ToString(),
                    SecretKey = Guid.NewGuid().ToString()
                };
                user.UserPermissions = permissions.Select(permission => new UserPermission
                {
                    UserId = user.UserId,
                    PermissionId = permission.PermissionId
                }).ToList();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await _authService.SendQrCodeViaEmail(dto);

                return "success";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
