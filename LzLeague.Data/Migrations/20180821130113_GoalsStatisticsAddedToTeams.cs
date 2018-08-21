using Microsoft.EntityFrameworkCore.Migrations;

namespace LzLeague.Data.Migrations
{
    public partial class GoalsStatisticsAddedToTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlayedMatchesCount",
                table: "Teams",
                newName: "GoalsScored");

            migrationBuilder.AddColumn<int>(
                name: "GoalsReceived",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoalsReceived",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "GoalsScored",
                table: "Teams",
                newName: "PlayedMatchesCount");
        }
    }
}
