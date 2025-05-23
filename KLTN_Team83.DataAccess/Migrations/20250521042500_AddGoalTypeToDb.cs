using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KLTN_Team83.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalTypeToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Goals",
                newName: "Id_GoalType");

            migrationBuilder.CreateTable(
                name: "GoalTypes",
                columns: table => new
                {
                    Id_GoalType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameGoalType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTypes", x => x.Id_GoalType);
                });

            migrationBuilder.InsertData(
                table: "GoalTypes",
                columns: new[] { "Id_GoalType", "NameGoalType" },
                values: new object[,]
                {
                    { 1, "Weight" },
                    { 2, "Height" },
                    { 3, "Sleep" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_Id_GoalType",
                table: "Goals",
                column: "Id_GoalType");

            migrationBuilder.AddForeignKey(
                name: "FK_Goals_GoalTypes_Id_GoalType",
                table: "Goals",
                column: "Id_GoalType",
                principalTable: "GoalTypes",
                principalColumn: "Id_GoalType",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goals_GoalTypes_Id_GoalType",
                table: "Goals");

            migrationBuilder.DropTable(
                name: "GoalTypes");

            migrationBuilder.DropIndex(
                name: "IX_Goals_Id_GoalType",
                table: "Goals");

            migrationBuilder.RenameColumn(
                name: "Id_GoalType",
                table: "Goals",
                newName: "Type");
        }
    }
}
