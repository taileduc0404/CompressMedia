using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class updatedb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlobContainerContainerId",
                table: "blobs",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$UpIY1miP3GBhTD0akjcSeuWGAoKb0zxnv09SukitTOl.A65Q5zbSC");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$d/rxdJ60AfrmdE1.Jdbhx.gVbUiqQvIknlU5FLBahZpTIppCxOJYy");

            migrationBuilder.CreateIndex(
                name: "IX_blobs_BlobContainerContainerId",
                table: "blobs",
                column: "BlobContainerContainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_blobs_blobContainers_BlobContainerContainerId",
                table: "blobs",
                column: "BlobContainerContainerId",
                principalTable: "blobContainers",
                principalColumn: "ContainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blobs_blobContainers_BlobContainerContainerId",
                table: "blobs");

            migrationBuilder.DropIndex(
                name: "IX_blobs_BlobContainerContainerId",
                table: "blobs");

            migrationBuilder.DropColumn(
                name: "BlobContainerContainerId",
                table: "blobs");

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
    }
}
