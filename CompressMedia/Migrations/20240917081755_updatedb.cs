using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "blobs");

            migrationBuilder.AddColumn<int>(
                name: "BlobDataId",
                table: "blobs",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$Ia6GfJgfly9FEnix6giESO/UhL6SStdtCWjYjUaFEDgp5ve9yPMyC");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$cQbb.QcPoOlXwEsf3b4dae01mXiXzjysvKT4J3HqHSq.c13xRLoAC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobDataId",
                table: "blobs");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "blobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$CBkgJec/fssweosxvZ.xwuGc8hWAbPTHWLg/cdfCh2T9rhwZQ4XwK");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$Wumg3SARv1E/EXrRP5wxGOweEydqEb1xKxlmYwLM2XNNPshaqrySa");
        }
    }
}
