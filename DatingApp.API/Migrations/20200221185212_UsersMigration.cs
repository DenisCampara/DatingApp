using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp.API.Migrations
{
    public partial class UsersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Intruduction",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Introduction",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Introduction",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Intruduction",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
