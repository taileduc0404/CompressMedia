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

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<BlobContainer> BlobContainers { get; set; }
		public DbSet<Blob> Blobs { get; set; }
		public DbSet<Tenant> Tenants { get; set; }
		public DbSet<Permission> Permissions { get; set; }
		public DbSet<UserPermission> UserPermissions { get; set; }
		public DbSet<RolePermission> RolePermissions { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Report> Report { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BlobContainer>().HasKey(c => c.ContainerId);
			modelBuilder.Entity<Blob>().HasKey(b => b.BlobId);
			modelBuilder.Entity<User>().HasKey(u => u.UserId);
			modelBuilder.Entity<Role>().HasKey(r => r.RoleId);
			modelBuilder.Entity<Tenant>().HasKey(t => t.TenantId);
			modelBuilder.Entity<Permission>().HasKey(p => p.PermissionId);
			modelBuilder.Entity<UserPermission>().HasKey(up => new { up.UserId, up.PermissionId });
			modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });

			modelBuilder.Entity<Like>()
			.HasKey(l => l.Id);

			modelBuilder.Entity<Report>(entity =>
			{
				entity.HasKey(r => r.ReportId);

				entity.Property(r => r.MediaId)
					  .IsRequired(false);

				entity.Property(r => r.UserId)
					  .IsRequired(true);

				entity.HasOne(r => r.User)
					  .WithMany()
					  .HasForeignKey(r => r.UserId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(r => r.Blob)
					  .WithMany()
					  .HasForeignKey(r => r.MediaId)
					  .OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(r => r.Tenant)
					  .WithMany()
					  .HasForeignKey(r => r.TenantId)
					  .OnDelete(DeleteBehavior.SetNull);

				entity.Property(r => r.ReportDate)
					  .HasDefaultValueSql("GETDATE()");
			});

			modelBuilder.Entity<Comment>()
			.HasOne(c => c.User)
			.WithMany(u => u.Comments)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Comment>()
				.HasOne(c => c.Blob)
				.WithMany(b => b.Comments)
				.HasForeignKey(c => c.BlobId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserPermission>()
				.HasKey(up => new { up.UserId, up.PermissionId });

			modelBuilder.Entity<UserPermission>()
				.HasOne(up => up.User)
				.WithMany(u => u.UserPermissions)
				.HasForeignKey(up => up.UserId);

			modelBuilder.Entity<UserPermission>()
				.HasOne(up => up.Permission)
				.WithMany(p => p.UserPermissions)
				.HasForeignKey(up => up.PermissionId);


			modelBuilder.Entity<UserPermission>()
				.HasOne(up => up.Permission)
				.WithMany(p => p.UserPermissions)
				.HasForeignKey(up => up.PermissionId);

			modelBuilder.Entity<RolePermission>()
				.HasOne(rp => rp.Role)
				.WithMany(r => r.RolePermissions)
				.HasForeignKey(rp => rp.RoleId);

			modelBuilder.Entity<RolePermission>()
				.HasOne(rp => rp.Permission)
				.WithMany(p => p.RolePermissions)
				.HasForeignKey(rp => rp.PermissionId);

			modelBuilder.Entity<BlobContainer>()
				.HasOne(bc => bc.User)
				.WithMany(u => u.Containers)
				.HasForeignKey(bc => bc.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Blob>()
				.HasOne(b => b.BlobContainer)
				.WithMany(bc => bc.Blobs)
				.HasForeignKey(b => b.ContainerId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Blob>()
				.HasOne(b => b.Tenant)
				.WithMany(t => t.Blobs)
				.HasForeignKey(b => b.TenantId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<BlobContainer>()
				.HasOne(bc => bc.Tenant)
				.WithMany(t => t.BlobContainers)
				.HasForeignKey(bc => bc.TenantId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Tenant)
				.WithMany(t => t.Users)
				.HasForeignKey(u => u.TenantId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Role>()
				.HasOne(r => r.Tenant)
				.WithMany(t => t.Roles)
				.HasForeignKey(r => r.TenantId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Username)
				.IsUnique();

			modelBuilder.Entity<User>().HasData(
				new User
				{
					UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
					Username = "superadmin",
					FirstName = "Super",
					LastName = "Admin",
					Email = "tai996507@gmail.com",
					PasswordHash = PasswordHasher.Hash("SuperAdmin@123"),
					SecretKey = Guid.NewGuid().ToString()
				}
			);

			modelBuilder.Entity<Permission>().HasData(
				new Permission { PermissionId = 1, PermissionName = "CreateTenant" },
				new Permission { PermissionId = 2, PermissionName = "CreateRole" },
				new Permission { PermissionId = 3, PermissionName = "AssignPermissionToRole" },
				new Permission { PermissionId = 4, PermissionName = "UploadMedia" },
				new Permission { PermissionId = 5, PermissionName = "ResizeMedia" },
				new Permission { PermissionId = 6, PermissionName = "CompressMedia" },
				new Permission { PermissionId = 7, PermissionName = "AddUser" },
				new Permission { PermissionId = 8, PermissionName = "DeleteUser" },
				new Permission { PermissionId = 9, PermissionName = "DeleteMedia" },
				new Permission { PermissionId = 10, PermissionName = "ViewMedia" },
				new Permission { PermissionId = 11, PermissionName = "DeleteTenant" },
				new Permission { PermissionId = 12, PermissionName = "DeleteRole" },
				new Permission { PermissionId = 13, PermissionName = "DetailRole" },
				new Permission { PermissionId = 14, PermissionName = "CreateContainer" },
				new Permission { PermissionId = 15, PermissionName = "DeleteContainer" },
				new Permission { PermissionId = 16, PermissionName = "GetAllUser" },
				new Permission { PermissionId = 17, PermissionName = "EditProfile" }
			);

			modelBuilder.Entity<UserPermission>().HasData(
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 1 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 2 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 3 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 4 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 5 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 6 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 7 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 8 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 9 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 10 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 11 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 12 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 13 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 14 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 15 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 16 },
				new UserPermission { UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6", PermissionId = 17 }
			);

			base.OnModelCreating(modelBuilder);
		}
	}
}
