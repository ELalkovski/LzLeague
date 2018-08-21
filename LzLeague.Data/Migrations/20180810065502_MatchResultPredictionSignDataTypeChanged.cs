using Microsoft.EntityFrameworkCore.Migrations;

namespace LzLeague.Data.Migrations
{
    public partial class MatchResultPredictionSignDataTypeChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WinnerSign",
                table: "MatchesResultsPredictions",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "WinnerSign",
                table: "MatchesResultsPredictions",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
