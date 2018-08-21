using Microsoft.EntityFrameworkCore.Migrations;

namespace LzLeague.Data.Migrations
{
    public partial class AddedScoresStatisticsToPrediction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuessedElTeams",
                table: "Predictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuessedGroupWinners",
                table: "Predictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuessedResults",
                table: "Predictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuessedScores",
                table: "Predictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GuessedSecondPlaces",
                table: "Predictions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchesPlayed",
                table: "Groups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuessedElTeams",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "GuessedGroupWinners",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "GuessedResults",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "GuessedScores",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "GuessedSecondPlaces",
                table: "Predictions");

            migrationBuilder.DropColumn(
                name: "MatchesPlayed",
                table: "Groups");
        }
    }
}
