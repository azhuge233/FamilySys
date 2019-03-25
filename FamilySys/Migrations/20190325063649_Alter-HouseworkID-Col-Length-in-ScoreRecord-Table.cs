using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class AlterHouseworkIDColLengthinScoreRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HouseworkID",
                table: "ScoreRecords",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HouseworkID",
                table: "ScoreRecords",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);
        }
    }
}
