using AspNetCoreHero.ToastNotification;
using CompressMedia.Data;
using CompressMedia.Repositories;
using CompressMedia.Repositories.Interfaces;
using CompressMedia.Services;
using Microsoft.AspNetCore.Http.Features;
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

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(30);
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

            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });



            builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 536870912;
            });


            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.GetTempPath()),
                RequestPath = "/tempfiles"
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
