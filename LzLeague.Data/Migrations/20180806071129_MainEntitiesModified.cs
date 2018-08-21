using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LzLeague.Data.Migrations
{
    public partial class MainEntitiesModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Predictions_PredictionId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Predictions_PredictionId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Predictions_PredictionId1",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Predictions_PredictionId2",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_PredictionId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_PredictionId1",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_PredictionId2",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Matches_PredictionId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PredictionId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PredictionId1",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PredictionId2",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "PredictionId",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchesResultsPredictions",
                columns: table => new
                {
                    PredictionId = table.Column<int>(nullable: false),
                    MatchId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: true),
                    WinnerSign = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesResultsPredictions", x => new { x.PredictionId, x.MatchId });
                    table.ForeignKey(
                        name: "FK_MatchesResultsPredictions_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchesResultsPredictions_Predictions_PredictionId",
                        column: x => x.PredictionId,
                        principalTable: "Predictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupsWinnersPredictions",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    PredictionId = table.Column<int>(nullable: false),
                    FirstPlace = table.Column<string>(nullable: true),
                    SecondPlace = table.Column<string>(nullable: true),
                    EuropaLeague = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupsWinnersPredictions", x => new { x.GroupId, x.PredictionId });
                    table.ForeignKey(
                        name: "FK_GroupsWinnersPredictions_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupsWinnersPredictions_Predictions_PredictionId",
                        column: x => x.PredictionId,
                        principalTable: "Predictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_GroupId",
                table: "Teams",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupsWinnersPredictions_PredictionId",
                table: "GroupsWinnersPredictions",
                column: "PredictionId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchesResultsPredictions_MatchId",
                table: "MatchesResultsPredictions",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Groups_GroupId",
                table: "Teams",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Groups_GroupId",
                table: "Teams");

            migrationBuilder.DropTable(
                name: "GroupsWinnersPredictions");

            migrationBuilder.DropTable(
                name: "MatchesResultsPredictions");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Teams_GroupId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Teams",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PredictionId",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PredictionId1",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PredictionId2",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PredictionId",
                table: "Matches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_PredictionId",
                table: "Teams",
                column: "PredictionId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_PredictionId1",
                table: "Teams",
                column: "PredictionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_PredictionId2",
                table: "Teams",
                column: "PredictionId2");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PredictionId",
                table: "Matches",
                column: "PredictionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Predictions_PredictionId",
                table: "Matches",
                column: "PredictionId",
                principalTable: "Predictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Predictions_PredictionId",
                table: "Teams",
                column: "PredictionId",
                principalTable: "Predictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Predictions_PredictionId1",
                table: "Teams",
                column: "PredictionId1",
                principalTable: "Predictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Predictions_PredictionId2",
                table: "Teams",
                column: "PredictionId2",
                principalTable: "Predictions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
