using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class addscorecolinUsertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Users",
                maxLength: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Houseworks",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houseworks", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Houseworks");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Users");
        }
    }
}
