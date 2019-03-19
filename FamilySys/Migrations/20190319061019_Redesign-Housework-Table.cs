using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class RedesignHouseworkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Houseworks",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 10, nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    FromID = table.Column<string>(maxLength: 10, nullable: true),
                    ToID = table.Column<string>(maxLength: 10, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    IsDone = table.Column<bool>(nullable: false)
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
        }
    }
}
