using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "blobContainers",
                columns: table => new
                {
                    ContainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blobContainers", x => x.ContainerId);
                    table.ForeignKey(
                        name: "FK_blobContainers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MediaPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompressDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medias", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_medias_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blobs",
                columns: table => new
                {
                    BlobId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainerId = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blobs", x => x.BlobId);
                    table.ForeignKey(
                        name: "FK_blobs_blobContainers_ContainerId",
                        column: x => x.ContainerId,
                        principalTable: "blobContainers",
                        principalColumn: "ContainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "blobMetadata",
                columns: table => new
                {
                    MetadataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlobId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blobMetadata", x => x.MetadataId);
                    table.ForeignKey(
                        name: "FK_blobMetadata_blobs_BlobId",
                        column: x => x.BlobId,
                        principalTable: "blobs",
                        principalColumn: "BlobId");
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "UserId", "Email", "FirstName", "LastName", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { "7a4fad07-84c6-4a6c-abc6-80b9948602a6", "admin@gmail.com", "Super", "Admin", "$2a$11$Roh6A0eqTCfkdQ6bAJ024OYWQcIkamesSEW7XyL/lz7znZhNrdEbu", "admin" },
                    { "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec", "taileduc0404@gmail.com", "Tai", "Le Duc", "$2a$11$1eKucI97vZQI5.KMb0WbKeMwjjBLX7tOvp8cLGyMgwxuP8jZlWEeu", "Le Duc Tai" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_blobContainers_UserId",
                table: "blobContainers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_blobMetadata_BlobId",
                table: "blobMetadata",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_blobs_ContainerId",
                table: "blobs",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_medias_UserId",
                table: "medias",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blobMetadata");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "blobs");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "blobContainers");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
