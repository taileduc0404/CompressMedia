using CompressMedia.CustomAuthentication;
using CompressMedia.Models;
using Microsoft.EntityFrameworkCore;

namespace CompressMedia.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Media> medias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Media>()
                .HasKey(u => u.MediaId);

            modelBuilder.Entity<Role>()
                .HasKey(u => u.RoleId);

            // quan hệ 1-n giữa User và Media
            modelBuilder.Entity<Media>()
                .HasOne(i => i.User)
                .WithMany(u => u.Medias)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // quan hệ n-n giữa User và Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    ur => ur.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    ur => ur.HasOne<User>().WithMany().HasForeignKey("UserId")
                );

            modelBuilder.Entity<User>()
                .HasData(

                new User
                {
                    UserId = "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                    Username = "Le Duc Tai",
                    FirstName = "Tai",
                    LastName = "Le Duc",
                    Email = "taileduc0404@gmail.com",
                    PasswordHash = PasswordHasher.HashPassword("Tai@123")
                },

                new User
                {
                    UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                    Username = "admin",
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = PasswordHasher.HashPassword("Admin@123")
                }
                );


            base.OnModelCreating(modelBuilder);
        }


    }
}
