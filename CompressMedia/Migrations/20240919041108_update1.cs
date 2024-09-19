using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "blobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$BPe5IbRoqj45PoNV2pFjm.EqZRAEeSM0ilVjB2HDElfiREDZeNDpu");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$WvGlGezA7NCIe7qESpu9qOc.Plmm8ri5HrXD3XzILpBUdgcZJaWIq");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "blobs");

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
    }
}
