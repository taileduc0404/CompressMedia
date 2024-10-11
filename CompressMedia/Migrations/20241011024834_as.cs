using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompressMedia.Migrations
{
    /// <inheritdoc />
    public partial class @as : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                columns: new[] { "PasswordHash", "SecretKey" },
                values: new object[] { "$2a$11$6qN5522UBFclI7Wl/YbF1uEjQkrlYLKqkB.9JMijMq0pUzDLVId2C", "7e09df72-e44b-4587-bd5b-45ea9a0cf3f9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: "7a4fad07-84c6-4a6c-abc6-80b9948602a6",
                columns: new[] { "PasswordHash", "SecretKey" },
                values: new object[] { "$2a$11$ONAHJzk8391dWOqI1yoAnuIVFDL0GlPfTe0QMC5pv5DGCCNomCw4y", "68ade12d-c98f-428e-b939-a4c1a2ff89de" });
        }
    }
}
