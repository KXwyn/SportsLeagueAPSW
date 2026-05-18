using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsLeague.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewTableMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSponsors_Tournament_TournamentId",
                table: "TournamentSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Teams_TeamId",
                table: "TournamentTeam");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentTeam",
                table: "TournamentTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Referee",
                table: "Referee");

            migrationBuilder.RenameTable(
                name: "TournamentTeam",
                newName: "TournamentTeams");

            migrationBuilder.RenameTable(
                name: "Tournament",
                newName: "Tournaments");

            migrationBuilder.RenameTable(
                name: "Referee",
                newName: "Referees");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeam_TournamentId_TeamId",
                table: "TournamentTeams",
                newName: "IX_TournamentTeams_TournamentId_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeam_TeamId",
                table: "TournamentTeams",
                newName: "IX_TournamentTeams_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentTeams",
                table: "TournamentTeams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Referees",
                table: "Referees",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    HomeTeamId = table.Column<int>(type: "int", nullable: false),
                    AwayTeamId = table.Column<int>(type: "int", nullable: false),
                    RefereeId = table.Column<int>(type: "int", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Venue = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Matchday = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Referees_RefereeId",
                        column: x => x.RefereeId,
                        principalTable: "Referees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_AwayTeamId",
                        column: x => x.AwayTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Teams_HomeTeamId",
                        column: x => x.HomeTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_AwayTeamId",
                table: "Matches",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HomeTeamId",
                table: "Matches",
                column: "HomeTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RefereeId",
                table: "Matches",
                column: "RefereeId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSponsors_Tournaments_TournamentId",
                table: "TournamentSponsors",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeams_Teams_TeamId",
                table: "TournamentTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeams_Tournaments_TournamentId",
                table: "TournamentTeams",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentSponsors_Tournaments_TournamentId",
                table: "TournamentSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeams_Teams_TeamId",
                table: "TournamentTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeams_Tournaments_TournamentId",
                table: "TournamentTeams");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentTeams",
                table: "TournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Referees",
                table: "Referees");

            migrationBuilder.RenameTable(
                name: "TournamentTeams",
                newName: "TournamentTeam");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "Tournament");

            migrationBuilder.RenameTable(
                name: "Referees",
                newName: "Referee");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeams_TournamentId_TeamId",
                table: "TournamentTeam",
                newName: "IX_TournamentTeam_TournamentId_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeams_TeamId",
                table: "TournamentTeam",
                newName: "IX_TournamentTeam_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentTeam",
                table: "TournamentTeam",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournament",
                table: "Tournament",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Referee",
                table: "Referee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentSponsors_Tournament_TournamentId",
                table: "TournamentSponsors",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Teams_TeamId",
                table: "TournamentTeam",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeam_Tournament_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId",
                principalTable: "Tournament",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
