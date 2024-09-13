using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class UpdateCompressDurationInMediaTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "CompressDuration",
				table: "medias",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(TimeSpan),
				oldType: "time");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$8hDpG5jd8Nd2j7BBfzo6DODr0TCL9gFxNeioKgO/Rs87SfE/q6zgG");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$BclYbjz.V2NW6BGHdCPtQ.zQ1EJGsBqSN9W3u.90lRmzLzf0AuAyW");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<TimeSpan>(
				name: "CompressDuration",
				table: "medias",
				type: "time",
				nullable: false,
				defaultValue: new TimeSpan(0, 0, 0, 0, 0),
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$75YkyW6yPlTryug5g5qH0uJGCVwckUoNwckg1AZS1snacJdKLXJyi");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$TfZzcPCYfP2OPr4ZwbRIN.WjVD0/YI8SDHygfoUfZRKXSsUhMD0sG");
		}
	}
}
