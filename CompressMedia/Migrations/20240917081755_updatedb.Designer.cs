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
    [Migration("20240917081755_updatedb")]
    partial class updatedb
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
                    b.Property<int>("BlobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlobId"));

                    b.Property<int?>("BlobDataId")
                        .HasColumnType("int");

                    b.Property<string>("BlobName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContainerId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ContainerId");

                    b.HasIndex("UserId");

                    b.ToTable("blobContainers");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobMetadata", b =>
                {
                    b.Property<int>("MetadataId")
                        .HasColumnType("int");

                    b.Property<string>("BlobName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DataType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MetadataId");

                    b.ToTable("blobMetadata");
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
                            PasswordHash = "$2a$11$cQbb.QcPoOlXwEsf3b4dae01mXiXzjysvKT4J3HqHSq.c13xRLoAC",
                            Username = "Le Duc Tai"
                        },
                        new
                        {
                            UserId = "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                            Email = "admin@gmail.com",
                            FirstName = "Super",
                            LastName = "Admin",
                            PasswordHash = "$2a$11$Ia6GfJgfly9FEnix6giESO/UhL6SStdtCWjYjUaFEDgp5ve9yPMyC",
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
                    b.HasOne("CompressMedia.Models.BlobContainer", "Container")
                        .WithMany("Blobs")
                        .HasForeignKey("ContainerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Container");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobContainer", b =>
                {
                    b.HasOne("CompressMedia.Models.User", "User")
                        .WithMany("Containers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("CompressMedia.Models.BlobMetadata", b =>
                {
                    b.HasOne("CompressMedia.Models.Blob", "Blob")
                        .WithOne("MetaData")
                        .HasForeignKey("CompressMedia.Models.BlobMetadata", "MetadataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blob");
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

            modelBuilder.Entity("CompressMedia.Models.Blob", b =>
                {
                    b.Navigation("MetaData");
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
