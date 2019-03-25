using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class AddScoreRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreRecords",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 5, nullable: false),
                    UserID = table.Column<string>(maxLength: 10, nullable: false),
                    HouseworkID = table.Column<string>(maxLength: 5, nullable: false),
                    RateID = table.Column<string>(maxLength: 5, nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreRecords", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreRecords");
        }
    }
}
