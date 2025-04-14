using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KLTN_Team83.Migrations
{
    /// <inheritdoc />
    public partial class seedtoblogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    id_Acc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loginName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passWord = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.id_Acc);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    id_Blog = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_Expert = table.Column<int>(type: "int", nullable: false),
                    tilte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.id_Blog);
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "id_Blog", "content", "id_Expert", "img", "ngayTao", "tilte" },
                values: new object[,]
                {
                    { 1, "user1@gmail.com", 1, "Nam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user1" },
                    { 2, "user1@gmail.com", 1, "Nam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user2" },
                    { 3, "user1@gmail.com", 1, "Nam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
