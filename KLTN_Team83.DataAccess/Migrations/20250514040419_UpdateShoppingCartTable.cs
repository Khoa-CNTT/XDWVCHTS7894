using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ShoppingCarts",
                newName: "Id_SP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id_SP",
                table: "ShoppingCarts",
                newName: "Id");
        }
    }
}
