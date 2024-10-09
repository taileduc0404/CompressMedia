using AspNetCoreHero.ToastNotification.Abstractions;
using CompressMedia.DTOs;
using CompressMedia.Models;
using CompressMedia.PermissionRequirement;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomAuth.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;

        public UserController(IUserService userService, INotyfService notyfService)
        {
            _userService = userService;
            _notyfService = notyfService;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [CustomPermission("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            Guid? tenantId = HttpContext.Items["TenantId"] as Guid?;
            IEnumerable<User> users = await _userService.GetAllUser(tenantId);
            if (users == null)
            {
                return RedirectToAction("AccessDenied");
            }

            IEnumerable<UserDto> usersDto = users.Select(user => new UserDto
            {
                Username = user.Username,
            });

            return View(usersDto);
        }

        [HttpGet]
        [CustomPermission("EditProfile")]
        public async Task<IActionResult> EditProfile(string username)
        {
            User user = await _userService.GetUserByName(username);
            if (user == null)
            {
                return RedirectToAction("AccessDenied");
            }
            UserDto userDto = new UserDto()
            {
                Id = user.UserId,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            return View(userDto);
        }

        [HttpPost]
        [CustomPermission("EditProfile")]
        public async Task<IActionResult> EditProfile(UserDto userDto)
        {

            if (ModelState.IsValid)
            {
                User userUpdate = await _userService.EditProfile(userDto);

                UserDto user = new UserDto()
                {
                    Id = userUpdate.UserId,
                    Username = userUpdate.Username,
                    FirstName = userUpdate.FirstName,
                    LastName = userUpdate.LastName,
                    Email = userUpdate.Email,

                };
                _notyfService.Success("Your profile has updated. Login Again Please!");
                return RedirectToAction("Index", "Home");
            }

            _notyfService.Success("Update failed. Login Again Please!");
            return RedirectToAction("Index", "Home");
        }

    }
}
