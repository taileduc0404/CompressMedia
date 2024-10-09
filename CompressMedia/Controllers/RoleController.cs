using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.PermissionRequirement;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CompressMedia.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly INotyfService _notyfService;
        private readonly IPermissionService _permissionService;
        public RoleController(IRoleService roleService, INotyfService notyfService, IPermissionService permissionService)
        {
            _roleService = roleService;
            _notyfService = notyfService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Index()
        {
            Guid? _tenantId = HttpContext.Items["TenantId"] as Guid?;

            IEnumerable<Role> roles = await _roleService.GetAllRoles(_tenantId);
            if (roles is null)
            {
                return View();
            }

            IEnumerable<RoleDto> rolesDto = roles!.Select(role => new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                TenantId = role.TenantId,
            });

            return View(rolesDto);
        }

        [HttpGet]
        [CustomPermission("CreateRole")]
        public IActionResult CreateRole(Guid tenantId)
        {
            RoleDto roleDto = new RoleDto
            {
                TenantId = tenantId
            };
            return View(roleDto);
        }

        [HttpPost]
        [CustomPermission("CreateRole")]
        public IActionResult CreateRole(RoleDto roleDto)
        {
            string result = _roleService.CreateRole(roleDto);

            if (result == null)
            {
                _notyfService.Error("Create role failed.");
                return RedirectToAction("Index");
            }

            _notyfService.Success("Create role successfully.");
            return RedirectToAction("Index");

        }

        [HttpGet]
        [CustomPermission("DeleteRole")]
        public IActionResult DeleteRole(int roleId)
        {
            if (roleId > 0)
            {
                RoleDto roleDto = new RoleDto()
                {
                    RoleId = roleId,
                };
                return View(roleDto);
            }
            return View();
        }

        [HttpPost]
        [CustomPermission("DeleteRole")]
        public IActionResult DeleteRole(RoleDto roleDto)
        {
            string result = _roleService.DeleteRole(roleDto.RoleId);

            if (result == null)
            {
                _notyfService.Error("Create role failed.");
                return RedirectToAction("Index");
            }

            _notyfService.Success("Create role successfully.");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [CustomPermission("AssignPermissionToRole")]
        public IActionResult AssignPermission(int roleId)
        {

            IEnumerable<Permission> permissions = _permissionService.GetAllPermissions();
            if (permissions is null)
            {
                return View();
            }

            IEnumerable<PermissionDto> permissionsDto = permissions!.Select(permission => new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                RoleId = roleId,
                PermissionDescription = permission.PermissionDescription
            });

            return View(permissionsDto);
        }

        [HttpPost]
        [CustomPermission("AssignPermissionToRole")]
        public IActionResult AssignPermission(int roleId, List<int> selectedPermissions)
        {
            if (selectedPermissions == null)
            {
                selectedPermissions = new List<int>();
            }

            _roleService.AssignPermissionsToRole(roleId, selectedPermissions);
            _notyfService.Success("Permissions assigned successfully.");

            return RedirectToAction("Index");
        }

        [HttpGet]
        [CustomPermission("DetailRole")]
        public async Task<IActionResult> Detail(int roleId)
        {
            var rolePermissions = await _roleService.GetRolePermission(roleId);

            var rolePermissionDtos = rolePermissions.Select(rp => new RolePermissionDto
            {
                PermissionId = rp.PermissionId,
                PermissionName = rp.PermissionName
            }).ToList();

            return View(rolePermissionDtos);
        }

    }
}
