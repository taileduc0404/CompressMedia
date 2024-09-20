﻿// <auto-generated />
using System;
using CompressMedia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompressMedia.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240920082043_AddSecretKeyInUserTable")]
    partial class AddSecretKeyInUserTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.HasKey("BlobId");

                    b.HasIndex("ContainerId");

                    b.ToTable("blobs");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.Property<int>("ContainerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContainerId"));

                    b.Property<string>("ContainerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ContainerId");

                    b.HasIndex("UserId");

                    b.ToTable("blobContainers");
                });

            modelBuilder.Entity("CompressMedia.Models.Media", b =>
                {
                    b.Property<int>("MediaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MediaId"));

                    b.Property<string>("CompressDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MediaPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MediaId");

                    b.HasIndex("UserId");

                    b.ToTable("medias");
                });

            modelBuilder.Entity("CompressMedia.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("CompressMedia.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("UserId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            UserId = "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                            Email = "taileduc0404@gmail.com",
                            FirstName = "Tai",
                            LastName = "Le Duc",
                            PasswordHash = "$2a$11$eMAz8vvkofvzig2hQ2w5AOYwPXdAp6QxZ/1EqCGB2iYvYW2B7c09u",
                            Username = "Le Duc Tai"
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            Email = "admin@gmail.com",
                            FirstName = "Super",
                            LastName = "Admin",
                            PasswordHash = "$2a$11$fGPAAOHg1JsPbw.YwfmN5eTc98q.odt5wbJCxZCwyr9loaQIzKmEe",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoleId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("CompressMedia.Models.Blob", b =>
                {
                    b.HasOne("CompressMedia.Models.BlobContainer", "BlobContainer")
                        .WithMany("Blobs")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BlobContainer");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Containers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.Media", b =>
                {
                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Medias")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserRole", b =>
                {
                    b.HasOne("CompressMedia.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompressMedia.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.Navigation("Blobs");
                });

            modelBuilder.Entity("CompressMedia.Models.User", b =>
                {
                    b.Navigation("Containers");

                    b.Navigation("Medias");
                });
#pragma warning restore 612, 618
        }
    }
}
