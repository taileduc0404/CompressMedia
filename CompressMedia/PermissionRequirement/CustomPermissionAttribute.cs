using CompressMedia.Data;
using CompressMedia.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompressMedia.PermissionRequirement
{
    public class CustomPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public CustomPermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            var userService = context.HttpContext.RequestServices.GetService<IUserService>();

            if (dbContext == null || userService == null)
            {
                Console.WriteLine("DbContext or UserService is null");
                context.Result = new StatusCodeResult(500);
                return;
            }

            var filter = new PermissionRequirementFilter(_permission, dbContext, userService);
            filter.OnAuthorization(context);
        }

    }
}
