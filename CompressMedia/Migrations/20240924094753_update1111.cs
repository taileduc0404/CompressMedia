using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class update1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "temporarySecretKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SecretKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporarySecretKeys", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$2egHOo8k1ldxdrlJqtycBOq0l5g/ULFkPIOz4VkMZc7HvexTb.FkC");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$c2c5b/S7AXbGlZAqp.3m9e5ONVaNx3R5SMoTO2sM5ULacadVdCOxa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "temporarySecretKeys");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$sr2ZofJP4QCICU1bZmKQaO1qPc5RLS6XtMiagx3LQvIENunGfxFMO");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$p6H6smTNKZfaYBqr3ertz.zyPrcLOOnYsavEON.9SKQCRNCVFDpim");
        }
    }
}
