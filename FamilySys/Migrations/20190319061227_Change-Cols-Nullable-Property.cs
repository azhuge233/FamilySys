using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class ChangeColsNullableProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Houseworks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FromID",
                table: "Houseworks",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Houseworks",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Houseworks",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FromID",
                table: "Houseworks",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Houseworks",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
