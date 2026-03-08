using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CalculatorDB_00001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SelectedOperations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSpaceOnConcatenation = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
