using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class update111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                column: "PasswordHash",
                value: "$2a$11$sr2ZofJP4QCICU1bZmKQaO1qPc5RLS6XtMiagx3LQvIENunGfxFMO");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "UserId",
                keyValue: "d079a7e0-fb1f-4ecd-89fe-403dca72d5ec",
                column: "PasswordHash",
                value: "$2a$11$p6H6smTNKZfaYBqr3ertz.zyPrcLOOnYsavEON.9SKQCRNCVFDpim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
