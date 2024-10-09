
namespace CompressMedia.Middlewares.Extension
{
    public static class TenantMiddlewareExtension
    {
        public static IApplicationBuilder UseTenantMiddlewareExtension(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TenantMiddleware>();
        }
    }
}
