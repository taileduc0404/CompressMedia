using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class update12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expiry",
                table: "temporarySecretKeys");

            migrationBuilder.RenameColumn(
                name: "SecretKey",
                table: "temporarySecretKeys",
                newName: "QrCodeUrl");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$imT/0TJnYXcuMOh8JFfPy.RsiRKNI2xFYwht3gzzZex4EHN1wTSyS");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$dIAZdS./tIWbjcKGw/j22ejB6MsdxAVz7oo8EFEhUgrNBVsVrs3y6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QrCodeUrl",
                table: "temporarySecretKeys",
                newName: "SecretKey");

            migrationBuilder.AddColumn<DateTime>(
                name: "Expiry",
                table: "temporarySecretKeys",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
