using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class AddRateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 5, nullable: false),
                    HouseworkID = table.Column<string>(nullable: false),
                    FromID = table.Column<string>(nullable: false),
                    ToID = table.Column<string>(nullable: false),
                    Star = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");
        }
    }
}
