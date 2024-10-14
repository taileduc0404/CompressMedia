using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class updateaaaa : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "UserName",
				table: "Comments");

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$HvGacZUZFa0QVWAjVWdEgezmh710qJ1pxhmgaO2RNZfw5ouz.PyC6", "17899506-c59d-4eb3-948e-84d852443860" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "UserName",
				table: "Comments",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$icWJ3d9rG3mPhFoKmaEzH.rdxaVhO2jDrzj8XdSgmey2Ey8YKv0O2", "9212bc3a-aa19-456f-9019-5ac0d1e70849" });
		}
	}
}
