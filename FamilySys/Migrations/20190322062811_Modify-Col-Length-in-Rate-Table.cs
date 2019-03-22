using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class ModifyColLengthinRateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToID",
                table: "Rates",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "HouseworkID",
                table: "Rates",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FromID",
                table: "Rates",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ToID",
                table: "Rates",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "HouseworkID",
                table: "Rates",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "FromID",
                table: "Rates",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);
        }
    }
}
