using System.Security.Claims;

namespace CompressMedia.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        //public async Task InvokeAsync(HttpContext context, IUserService _userService, ApplicationDbContext _context)
        {

            string? userName = context.User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(userName))
            {
                await _next(context);
                return;
            }

            //User? userLogin = _context.Users.FirstOrDefault(u => u.Username == userName);

            //if (userLogin == null)
            //{
            //    context.Items["TenantId"] = null;
            //    await _next(context);
            //    return;
            //}

            //Guid? tenantId = userLogin.TenantId;
            //context.Items["TenantId"] = tenantId;

            await _next(context);
        }
    }
}
