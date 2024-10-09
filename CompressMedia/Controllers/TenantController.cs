using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.PermissionRequirement;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompressMedia.Controllers
{
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly IRoleService _roleService;
        private readonly IAuthService _authService;
        private readonly INotyfService _notyfService;

        public TenantController(ITenantService tenantService, INotyfService notyfService, IAuthService authService, IRoleService roleService)
        {
            _tenantService = tenantService;
            _notyfService = notyfService;
            _authService = authService;
            _roleService = roleService;
        }


        [HttpGet]
        [CustomPermission("CreateTenant")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Tenant> tenants = await _tenantService.GetAllAsync();
            if (tenants is null)
            {
                _notyfService.Error("No role.");
            }

            IEnumerable<TenantDto> tenantDtos = tenants!.Select(tenant => new TenantDto
            {
                TenantId = tenant.TenantId,
                TenantName = tenant.TenantName
            });

            return View(tenantDtos);
        }

        [HttpGet]
        [CustomPermission("CreateTenant")]
        public IActionResult CreateTenant()
        {
            return View(new TenantDto());
        }

        [HttpPost]
        [CustomPermission("CreateTenant")]
        public async Task<IActionResult> CreateTenant(TenantDto tenantDto)
        {
            if (tenantDto is null)
            {
                _notyfService.Error("Cannot create tenant.");
                return RedirectToAction("Index");
            }
            await _tenantService.CreateTenantAsync(tenantDto);
            _notyfService.Success($"Tenant {tenantDto.TenantName} create successfully.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [CustomPermission("AddUser")]
        public async Task<IActionResult> AddUser()
        {
            Guid? _tenantId = HttpContext.Items["TenantId"] as Guid?;
            IEnumerable<Role> roles = await _roleService.GetAllRoles(_tenantId);

            var registerDto = new RegisterDto
            {
                Roles = roles.Select(r => new RoleDto { RoleId = r.RoleId, RoleName = r.RoleName }).ToList()
            };
            return View(registerDto);
        }

        [HttpPost]
        [CustomPermission("AddUser")]
        public async Task<IActionResult> AddUser(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Warning("Please enter your info.");
                return View(nameof(AddUser));
            }

            Guid? _tenantId = HttpContext.Items["TenantId"] as Guid?;
            var result = await _tenantService.AddUser(dto, _tenantId);

            switch (result)
            {
                case "null":
                    _notyfService.Error("Register failed.");
                    return View(nameof(AddUser));
                case "usernameExist":
                    _notyfService.Warning("Username or email you enter already exist.");
                    return View(nameof(AddUser));
                default:
                    _notyfService.Success("Register successfully. Check your email and scan qr code to login");
                    return RedirectToAction("GetAllUser", "User");
            }
        }
    }
}
