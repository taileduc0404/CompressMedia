using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
	/// <inheritdoc />
	public partial class first1 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "blobMetadata");

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

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "blobMetadata",
				columns: table => new
				{
					MetadataId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BlobId = table.Column<string>(type: "nvarchar(450)", nullable: true),
					BlobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DataType = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_blobMetadata", x => x.MetadataId);
					table.ForeignKey(
						name: "FK_blobMetadata_blobs_BlobId",
						column: x => x.BlobId,
						principalTable: "blobs",
						principalColumn: "BlobId");
				});

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
				column: "PasswordHash",
				value: "$2a$11$Roh6A0eqTCfkdQ6bAJ024OYWQcIkamesSEW7XyL/lz7znZhNrdEbu");

			migrationBuilder.UpdateData(
				table: "users",
				keyColumn: "UserId",
				keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
				column: "PasswordHash",
				value: "$2a$11$1eKucI97vZQI5.KMb0WbKeMwjjBLX7tOvp8cLGyMgwxuP8jZlWEeu");

			migrationBuilder.CreateIndex(
				name: "IX_blobMetadata_BlobId",
				table: "blobMetadata",
				column: "BlobId");
		}
	}
}
