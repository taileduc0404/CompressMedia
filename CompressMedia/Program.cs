using AspNetCoreHero.ToastNotification;
using CompressMedia.Data;
using CompressMedia.Middlewares.Extension;
using CompressMedia.Repositories;
using CompressMedia.Repositories.Interfaces;
using CompressMedia.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CompressMedia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddSingleton<BlobStorageDbContext>(provider =>
            {
                var configuration = builder.Configuration;
                string connectionString = configuration["BlobDatabase:ConnectionString"]!;
                string databaseName = configuration["BlobDatabase:DatabaseName"]!;
                return new BlobStorageDbContext(connectionString, databaseName);
            });

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMediaService, MediaService>();
            builder.Services.AddScoped<IBlobContainerService, BlobContainerService>();
            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped<IImageResizer, ImageResizer>();
            builder.Services.AddScoped<IImageSharpService, ImageSharpService>();
            builder.Services.AddScoped<IImageFFmpegService, ImageFFmpegService>();
            builder.Services.AddScoped<ICompressService, CompressService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();

            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 15;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var authService = scopeService.GetRequiredService<IAuthService>();
                authService.SeedSuperAdmin().GetAwaiter().GetResult();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMp4FileValidationMiddleware();

            app.UseTenantMiddlewareExtension();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
