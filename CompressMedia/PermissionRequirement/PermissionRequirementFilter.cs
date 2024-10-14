using CompressMedia.Data;
using CompressMedia.Models;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.PermissionRequirement
{
    public class PermissionRequirementFilter : IAuthorizationFilter
    {
        private readonly string _permission;
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        public PermissionRequirementFilter(string permission, ApplicationDbContext context, IUserService userService)
        {
            _permission = permission;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string userName = _userService!.GetUserNameLoggedIn();

            if (string.IsNullOrEmpty(userName))
            {
                context.Result = new ForbidResult();
                return;
            }

            User? userLoggedIn = _context!.Users.FirstOrDefault(u => u.Username == userName);

            if (userLoggedIn == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userPermissions = _context.UserPermissions
                .Include(up => up.Permission)
                .Where(up => up.UserId == userLoggedIn.UserId)
                .Select(up => up.Permission)
                .ToList();

            if (!userPermissions.Any(p => p!.PermissionName == _permission))
            {
                context.Result = new ForbidResult();
            }
        }

    }
}
