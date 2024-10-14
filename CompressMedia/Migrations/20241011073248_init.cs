﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class init : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Permissions",
				columns: table => new
				{
					PermissionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PermissionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Permissions", x => x.PermissionId);
				});

			migrationBuilder.CreateTable(
				name: "Tenants",
				columns: table => new
				{
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tenants", x => x.TenantId);
				});

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					RoleId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.RoleId);
					table.ForeignKey(
						name: "FK_Roles_Tenants_TenantId",
						column: x => x.TenantId,
						principalTable: "Tenants",
						principalColumn: "TenantId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "PermissionRole",
				columns: table => new
				{
					PermissionsPermissionId = table.Column<int>(type: "int", nullable: false),
					RolesRoleId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PermissionRole", x => new { x.PermissionsPermissionId, x.RolesRoleId });
					table.ForeignKey(
						name: "FK_PermissionRole_Permissions_PermissionsPermissionId",
						column: x => x.PermissionsPermissionId,
						principalTable: "Permissions",
						principalColumn: "PermissionId",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_PermissionRole_Roles_RolesRoleId",
						column: x => x.RolesRoleId,
						principalTable: "Roles",
						principalColumn: "RoleId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "RolePermissions",
				columns: table => new
				{
					RoleId = table.Column<int>(type: "int", nullable: false),
					PermissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
					table.ForeignKey(
						name: "FK_RolePermissions_Permissions_PermissionId",
						column: x => x.PermissionId,
						principalTable: "Permissions",
						principalColumn: "PermissionId",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_RolePermissions_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "RoleId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					RoleId = table.Column<int>(type: "int", nullable: true),
					Username = table.Column<string>(type: "nvarchar(450)", nullable: true),
					FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
					PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
					SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserId);
					table.ForeignKey(
						name: "FK_Users_Roles_RoleId",
						column: x => x.RoleId,
						principalTable: "Roles",
						principalColumn: "RoleId");
					table.ForeignKey(
						name: "FK_Users_Tenants_TenantId",
						column: x => x.TenantId,
						principalTable: "Tenants",
						principalColumn: "TenantId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "BlobContainers",
				columns: table => new
				{
					ContainerId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BlobContainers", x => x.ContainerId);
					table.ForeignKey(
						name: "FK_BlobContainers_Tenants_TenantId",
						column: x => x.TenantId,
						principalTable: "Tenants",
						principalColumn: "TenantId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_BlobContainers_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "UserPermissions",
				columns: table => new
				{
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					PermissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
					table.ForeignKey(
						name: "FK_UserPermissions_Permissions_PermissionId",
						column: x => x.PermissionId,
						principalTable: "Permissions",
						principalColumn: "PermissionId",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_UserPermissions_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Blobs",
				columns: table => new
				{
					BlobId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					BlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Size = table.Column<double>(type: "float", nullable: false),
					CompressionTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ContainerId = table.Column<int>(type: "int", nullable: false),
					UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Blobs", x => x.BlobId);
					table.ForeignKey(
						name: "FK_Blobs_BlobContainers_ContainerId",
						column: x => x.ContainerId,
						principalTable: "BlobContainers",
						principalColumn: "ContainerId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Blobs_Tenants_TenantId",
						column: x => x.TenantId,
						principalTable: "Tenants",
						principalColumn: "TenantId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Blobs_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId");
				});

			migrationBuilder.InsertData(
				table: "Permissions",
				columns: new[] { "PermissionId", "PermissionDescription", "PermissionName" },
				values: new object[,]
				{
					{ 1, null, "CreateTenant" },
					{ 2, null, "CreateRole" },
					{ 3, null, "AssignPermissionToRole" },
					{ 4, null, "UploadMedia" },
					{ 5, null, "ResizeMedia" },
					{ 6, null, "CompressMedia" },
					{ 7, null, "AddUser" },
					{ 8, null, "DeleteUser" },
					{ 9, null, "DeleteMedia" },
					{ 10, null, "ViewMedia" },
					{ 11, null, "DeleteTenant" },
					{ 12, null, "DeleteRole" },
					{ 13, null, "DetailRole" },
					{ 14, null, "CreateContainer" },
					{ 15, null, "DeleteContainer" },
					{ 16, null, "GetAllUser" },
					{ 17, null, "EditProfile" }
				});

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "UserId", "Email", "FirstName", "LastName", "PasswordHash", "RoleId", "SecretKey", "TenantId", "Username" },
				values: new object[] { "7a4fad07-84c6-4a6c-abc6-80b9948602a6", "tai996507@gmail.com", "Super", "Admin", "$2a$11$PeGw2evl4tNTXhMc9PVdT.jCiPdjPM0yYxE.fTYzrk.9DhwyR8BsC", null, "336db662-aa22-4dc2-9993-774efa0e8b72", null, "superadmin" });

			migrationBuilder.InsertData(
				table: "UserPermissions",
				columns: new[] { "PermissionId", "UserId" },
				values: new object[,]
				{
					{ 1, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 2, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 3, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 4, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 5, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 6, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 7, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 8, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 9, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 10, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 11, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 12, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 13, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 14, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 15, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 16, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" },
					{ 17, "7a4fad07-84c6-4a6c-abc6-80b9948602a6" }
				});

			migrationBuilder.CreateIndex(
				name: "IX_BlobContainers_TenantId",
				table: "BlobContainers",
				column: "TenantId");

			migrationBuilder.CreateIndex(
				name: "IX_BlobContainers_UserId",
				table: "BlobContainers",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Blobs_ContainerId",
				table: "Blobs",
				column: "ContainerId");

			migrationBuilder.CreateIndex(
				name: "IX_Blobs_TenantId",
				table: "Blobs",
				column: "TenantId");

			migrationBuilder.CreateIndex(
				name: "IX_Blobs_UserId",
				table: "Blobs",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_PermissionRole_RolesRoleId",
				table: "PermissionRole",
				column: "RolesRoleId");

			migrationBuilder.CreateIndex(
				name: "IX_RolePermissions_PermissionId",
				table: "RolePermissions",
				column: "PermissionId");

			migrationBuilder.CreateIndex(
				name: "IX_Roles_TenantId",
				table: "Roles",
				column: "TenantId");

			migrationBuilder.CreateIndex(
				name: "IX_UserPermissions_PermissionId",
				table: "UserPermissions",
				column: "PermissionId");

			migrationBuilder.CreateIndex(
				name: "IX_Users_RoleId",
				table: "Users",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "IX_Users_TenantId",
				table: "Users",
				column: "TenantId");

			migrationBuilder.CreateIndex(
				name: "IX_Users_Username",
				table: "Users",
				column: "Username",
				unique: true,
				filter: "[Username] IS NOT NULL");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Blobs");

			migrationBuilder.DropTable(
				name: "PermissionRole");

			migrationBuilder.DropTable(
				name: "RolePermissions");

			migrationBuilder.DropTable(
				name: "UserPermissions");

			migrationBuilder.DropTable(
				name: "BlobContainers");

			migrationBuilder.DropTable(
				name: "Permissions");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "Roles");

			migrationBuilder.DropTable(
				name: "Tenants");
		}
	}
}
