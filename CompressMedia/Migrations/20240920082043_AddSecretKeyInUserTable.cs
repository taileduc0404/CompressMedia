using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class AddSecretKeyInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                columns: new[] { "PasswordHash", "SecretKey" },
                values: new object[] { "$2a$11$fGPAAOHg1JsPbw.YwfmN5eTc98q.odt5wbJCxZCwyr9loaQIzKmEe", null });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                columns: new[] { "PasswordHash", "SecretKey" },
                values: new object[] { "$2a$11$eMAz8vvkofvzig2hQ2w5AOYwPXdAp6QxZ/1EqCGB2iYvYW2B7c09u", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "users");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$sh.Aef870K1eKR1V.pQU0uOshJQuqnN2.5inN9EjYxWn/Z2QL5yw6");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$OSp/nnvkCjXKf/63piaSx..b6yG6R4dSR.LwgqRc6nPmPBv45xBwm");
        }
    }
}
