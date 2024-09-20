using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class update2 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$Lb/FiG1ey9Nn6L4KD1vUReZwtsoRKChREEXmoomAJSXpXHnFDmWnq");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$380.uYfUJHEVbZ6IU5VtTOegde49xpBHiTXxBIb5s2Q7J.1E1znKG");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
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
	}
}
