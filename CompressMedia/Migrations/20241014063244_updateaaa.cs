using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class updateaaa : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "CommentId1",
				table: "Comments",
				type: "int",
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "ParentComment",
				table: "Comments",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$icWJ3d9rG3mPhFoKmaEzH.rdxaVhO2jDrzj8XdSgmey2Ey8YKv0O2", "9212bc3a-aa19-456f-9019-5ac0d1e70849" });

			migrationBuilder.CreateIndex(
				name: "IX_Comments_CommentId1",
				table: "Comments",
				column: "CommentId1");

			migrationBuilder.AddForeignKey(
				name: "FK_Comments_Comments_CommentId1",
				table: "Comments",
				column: "CommentId1",
				principalTable: "Comments",
				principalColumn: "CommentId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Comments_Comments_CommentId1",
				table: "Comments");

			migrationBuilder.DropIndex(
				name: "IX_Comments_CommentId1",
				table: "Comments");

			migrationBuilder.DropColumn(
				name: "CommentId1",
				table: "Comments");

			migrationBuilder.DropColumn(
				name: "ParentComment",
				table: "Comments");

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$tArnv4EIHKfhbCD1OWuztuse03zhI.eC1o/9CIFeePdO1nVJp3GFW", "df9dbc09-e043-451c-945a-2752450302ae" });
		}
	}
}
