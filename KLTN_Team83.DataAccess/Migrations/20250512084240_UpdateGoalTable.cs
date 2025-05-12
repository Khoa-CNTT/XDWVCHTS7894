using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Goals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ApplicationUserId",
                table: "Goals",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_AspNetUsers_ApplicationUserId",
                table: "Goals",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_AspNetUsers_ApplicationUserId",
                table: "Goals");

            migrationBuilder.DropIndex(
                name: "IX_Goals_ApplicationUserId",
                table: "Goals");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Goals");
        }
    }
}
