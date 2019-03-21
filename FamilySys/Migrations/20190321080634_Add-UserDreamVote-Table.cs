using Microsoft.EntityFrameworkCore.Migrations;

namespace FamilySys.Migrations
{
    public partial class AddUserDreamVoteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDreamVotes",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 5, nullable: false),
                    UserID = table.Column<string>(maxLength: 10, nullable: false),
                    DreamID = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDreamVotes", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDreamVotes");
        }
    }
}
