using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCompanyToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Company",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id_Company = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id_Company);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Id_Company",
                table: "AspNetUsers",
                column: "Id_Company");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_Id_Company",
                table: "AspNetUsers",
                column: "Id_Company",
                principalTable: "Companies",
                principalColumn: "Id_Company");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_Id_Company",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Id_Company",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id_Company",
                table: "AspNetUsers");
        }
    }
}
