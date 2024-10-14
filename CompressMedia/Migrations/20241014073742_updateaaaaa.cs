using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class updateaaaaa : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Report",
				columns: table => new
				{
					ReportId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					MediaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
					ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Report", x => x.ReportId);
					table.ForeignKey(
						name: "FK_Report_Blobs_MediaId",
						column: x => x.MediaId,
						principalTable: "Blobs",
						principalColumn: "BlobId",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_Report_Tenants_TenantId",
						column: x => x.TenantId,
						principalTable: "Tenants",
						principalColumn: "TenantId",
						onDelete: ReferentialAction.SetNull);
					table.ForeignKey(
						name: "FK_Report_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$AhYH.dvAcDj8AKoqHi8pbOrNGKfqaoVWY585WXRajRFvGDXduruZ6", "076d16d5-2576-475b-b867-53be2d3ce156" });

			migrationBuilder.CreateIndex(
				name: "IX_Report_MediaId",
				table: "Report",
				column: "MediaId");

			migrationBuilder.CreateIndex(
				name: "IX_Report_TenantId",
				table: "Report",
				column: "TenantId");

			migrationBuilder.CreateIndex(
				name: "IX_Report_UserId",
				table: "Report",
				column: "UserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Report");

			migrationBuilder.UpdateData(
				table: "Users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				columns: new[] { "PasswordHash", "SecretKey" },
				values: new object[] { "$2a$11$HvGacZUZFa0QVWAjVWdEgezmh710qJ1pxhmgaO2RNZfw5ouz.PyC6", "17899506-c59d-4eb3-948e-84d852443860" });
		}
	}
}
