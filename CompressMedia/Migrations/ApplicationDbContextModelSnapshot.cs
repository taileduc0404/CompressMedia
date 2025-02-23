﻿// <auto-generated />
using System;
using CompressMedia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompressMedia.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompressMedia.Models.Blob", b =>
                {
                    b.Property<string>("BlobId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BlobName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompressionTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContainerId")
                        .HasColumnType("int");

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Size")
                        .HasColumnType("float");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BlobId");

                    b.HasIndex("ContainerId");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("Blobs");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.Property<int>("ContainerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContainerId"));

                    b.Property<string>("ContainerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ContainerId");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("BlobContainers");
                });

            modelBuilder.Entity("CompressMedia.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<string>("BlobId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CommentId1")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ParentComment")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommentId");

                    b.HasIndex("BlobId");

                    b.HasIndex("CommentId1");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CompressMedia.Models.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BlobId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LikedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("BlobId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("CompressMedia.Models.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermissionId"));

                    b.Property<string>("PermissionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermissionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PermissionId");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            PermissionId = 1,
                            PermissionName = "CreateTenant"
                        },
                        new
                        {
                            PermissionId = 2,
                            PermissionName = "CreateRole"
                        },
                        new
                        {
                            PermissionId = 3,
                            PermissionName = "AssignPermissionToRole"
                        },
                        new
                        {
                            PermissionId = 4,
                            PermissionName = "UploadMedia"
                        },
                        new
                        {
                            PermissionId = 5,
                            PermissionName = "ResizeMedia"
                        },
                        new
                        {
                            PermissionId = 6,
                            PermissionName = "CompressMedia"
                        },
                        new
                        {
                            PermissionId = 7,
                            PermissionName = "AddUser"
                        },
                        new
                        {
                            PermissionId = 8,
                            PermissionName = "DeleteUser"
                        },
                        new
                        {
                            PermissionId = 9,
                            PermissionName = "DeleteMedia"
                        },
                        new
                        {
                            PermissionId = 10,
                            PermissionName = "ViewMedia"
                        },
                        new
                        {
                            PermissionId = 11,
                            PermissionName = "DeleteTenant"
                        },
                        new
                        {
                            PermissionId = 12,
                            PermissionName = "DeleteRole"
                        },
                        new
                        {
                            PermissionId = 13,
                            PermissionName = "DetailRole"
                        },
                        new
                        {
                            PermissionId = 14,
                            PermissionName = "CreateContainer"
                        },
                        new
                        {
                            PermissionId = 15,
                            PermissionName = "DeleteContainer"
                        },
                        new
                        {
                            PermissionId = 16,
                            PermissionName = "GetAllUser"
                        },
                        new
                        {
                            PermissionId = 17,
                            PermissionName = "EditProfile"
                        },
                        new
                        {
                            PermissionId = 18,
                            PermissionName = "ViewTenant"
                        },
                        new
                        {
                            PermissionId = 19,
                            PermissionName = "ViewReport"
                        },
                        new
                        {
                            PermissionId = 20,
                            PermissionName = "Report"
                        },
                        new
                        {
                            PermissionId = 21,
                            PermissionName = "ViewContainer"
                        });
                });

            modelBuilder.Entity("CompressMedia.Models.Report", b =>
                {
                    b.Property<int>("ReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReportId"));

                    b.Property<string>("MediaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ReportDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ReportId");

                    b.HasIndex("MediaId");

                    b.HasIndex("TenantId");

                    b.HasIndex("UserId");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("CompressMedia.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId");

                    b.HasIndex("TenantId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CompressMedia.Models.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("CompressMedia.Models.Tenant", b =>
                {
                    b.Property<Guid>("TenantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TenantId");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("CompressMedia.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("TenantId");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            Email = "tai996507@gmail.com",
                            FirstName = "Super",
                            LastName = "Admin",
                            PasswordHash = "$2a$11$p4a0J1AA85ZYmLrfJdmwp.Nf8L1xZ7MxrVjAFmLeojK08siFuTnGW",
                            SecretKey = "ed861a5e-3fab-4290-a879-cd5da35415c9",
                            Username = "superadmin"
                        });
                });

            modelBuilder.Entity("CompressMedia.Models.UserPermission", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("UserPermissions");

                    b.HasData(
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 1
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 2
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 3
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 4
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 5
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 6
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 7
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 8
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 9
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 10
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 11
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 12
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 13
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 14
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 15
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 16
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 17
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 18
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 19
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 20
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            PermissionId = 21
                        });
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsPermissionId")
                        .HasColumnType("int");

                    b.Property<int>("RolesRoleId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsPermissionId", "RolesRoleId");

                    b.HasIndex("RolesRoleId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("CompressMedia.Models.Blob", b =>
                {
                    b.HasOne("CompressMedia.Models.BlobContainer", "BlobContainer")
                        .WithMany("Blobs")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CompressMedia.Models.Tenant", "Tenant")
                        .WithMany("Blobs")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Blobs")
                        .HasForeignKey("UserId");

                    b.Navigation("BlobContainer");

                    b.Navigation("Tenant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.HasOne("CompressMedia.Models.Tenant", "Tenant")
                        .WithMany("BlobContainers")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Containers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Tenant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.Comment", b =>
                {
                    b.HasOne("CompressMedia.Models.Blob", "Blob")
                        .WithMany("Comments")
                        .HasForeignKey("BlobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompressMedia.Models.Comment", null)
                        .WithMany("ChildComments")
                        .HasForeignKey("CommentId1");

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Blob");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.Like", b =>
                {
                    b.HasOne("CompressMedia.Models.Blob", "Blob")
                        .WithMany()
                        .HasForeignKey("BlobId");

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Blob");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.Report", b =>
                {
                    b.HasOne("CompressMedia.Models.Blob", "Blob")
                        .WithMany()
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CompressMedia.Models.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blob");

                    b.Navigation("Tenant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.Role", b =>
                {
                    b.HasOne("CompressMedia.Models.Tenant", "Tenant")
                        .WithMany("Roles")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("CompressMedia.Models.RolePermission", b =>
                {
                    b.HasOne("CompressMedia.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompressMedia.Models.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CompressMedia.Models.User", b =>
                {
                    b.HasOne("CompressMedia.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.HasOne("CompressMedia.Models.Tenant", "Tenant")
                        .WithMany("Users")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Role");

                    b.Navigation("Tenant");
                });

            modelBuilder.Entity("CompressMedia.Models.UserPermission", b =>
                {
                    b.HasOne("CompressMedia.Models.Permission", "Permission")
                        .WithMany("UserPermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("UserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("CompressMedia.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsPermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompressMedia.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompressMedia.Models.Blob", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.Navigation("Blobs");
                });

            modelBuilder.Entity("CompressMedia.Models.Comment", b =>
                {
                    b.Navigation("ChildComments");
                });

            modelBuilder.Entity("CompressMedia.Models.Permission", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("CompressMedia.Models.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CompressMedia.Models.Tenant", b =>
                {
                    b.Navigation("BlobContainers");

                    b.Navigation("Blobs");

                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CompressMedia.Models.User", b =>
                {
                    b.Navigation("Blobs");

                    b.Navigation("Comments");

                    b.Navigation("Containers");

                    b.Navigation("UserPermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
