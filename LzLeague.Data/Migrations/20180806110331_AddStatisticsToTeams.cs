using Microsoft.EntityFrameworkCore.Migrations;

namespace LzLeague.Data.Migrations
{
    public partial class AddStatisticsToTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Draws",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Loses",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Draws",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Loses",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "Teams");
        }
    }
}
