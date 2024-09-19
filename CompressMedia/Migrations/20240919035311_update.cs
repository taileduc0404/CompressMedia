using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompressionTime",
                table: "blobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "blobs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$xAv4DWWPdQEWLvanlWu7CubNgWaMIkRl0ydYbwDTzCVzU8VTMmWxi");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$Wd192kx6qxePFubAHh1yWuVaetpcR3/GAX.psK5JMGAVEbysBelLi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompressionTime",
                table: "blobs");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "blobs");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$Uc7hQeM1c2LVeRe8qPLBQ.22Pyw0SweaBKkLd9Ijm.vSN.VQ12sYa");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$9MRJ8J4vO3Sq0ppPwUtjp.klnb1wMmmCyesZidL.uWgdCT2gOQ10O");
        }
    }
}
