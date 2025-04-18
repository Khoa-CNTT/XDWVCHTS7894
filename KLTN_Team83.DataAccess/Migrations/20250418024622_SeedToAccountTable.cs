﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedToAccountTable : Migration
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
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    tilte = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.id_Blog);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "id_Acc", "email", "passWord", "role" },
                values: new object[,]
                {
                    { 1, "user1@gmail.com", "jshafsgh", "user" },
                    { 2, "user2@gmail.com", "hsfagjs", "user" }
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
