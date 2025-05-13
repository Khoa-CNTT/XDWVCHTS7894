using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id_Product",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id_Product",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id_Product",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id_Product", "Description", "Id_Category", "ImgageUrl", "NCC", "NameProduct", "Price", "Price100", "Price50" },
                values: new object[,]
                {
                    { 1, "Thịt bò tươi ngon", 1, "", "Đá Chẹt", "Thịt bò", 20.0, 15.0, 18.0 },
                    { 2, "Nước điện giải có ga bù nước", 2, "", "Long Châu", "Nước điện giải", 10.0, 5.0, 8.0 },
                    { 3, "Kẹo có thuốc xổ", 4, "", "Quang Linh", "Kẹo rau Kera", 50.0, 43.0, 45.0 }
                });
        }
    }
}
