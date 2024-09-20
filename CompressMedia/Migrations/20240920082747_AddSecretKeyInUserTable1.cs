using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class AddSecretKeyInUserTable1 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_users_Username",
				table: "users");

			migrationBuilder.AlterColumn<string>(
				name: "Username",
				table: "users",
				type: "nvarchar(450)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(10)",
				oldMaxLength: 10);

			migrationBuilder.AlterColumn<string>(
				name: "PasswordHash",
				table: "users",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "LastName",
				table: "users",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "FirstName",
				table: "users",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "users",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$FvLbtcR3iTgM2fWVpYElE.gp38M80BvWKz6oUtGhLsPxfVfhhc132");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$S0wyPocGUAY9fp4i5tKBMOVz5f.lk6gdD9nYHV4EaaiTgOjoqNRhq");

			migrationBuilder.CreateIndex(
				name: "IX_users_Username",
				table: "users",
				column: "Username",
				unique: true,
				filter: "[Username] IS NOT NULL");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_users_Username",
				table: "users");

			migrationBuilder.AlterColumn<string>(
				name: "Username",
				table: "users",
				type: "nvarchar(10)",
				maxLength: 10,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(450)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "PasswordHash",
				table: "users",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "LastName",
				table: "users",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "FirstName",
				table: "users",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Email",
				table: "users",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$fGPAAOHg1JsPbw.YwfmN5eTc98q.odt5wbJCxZCwyr9loaQIzKmEe");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$eMAz8vvkofvzig2hQ2w5AOYwPXdAp6QxZ/1EqCGB2iYvYW2B7c09u");

			migrationBuilder.CreateIndex(
				name: "IX_users_Username",
				table: "users",
				column: "Username",
				unique: true);
		}
	}
}
