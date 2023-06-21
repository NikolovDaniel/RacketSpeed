using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketSpeed.Infrastructure.Migrations
{
    public partial class AddedDifferentRankingPropertiesToPlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ranking",
                table: "Players",
                newName: "WorldRanking");

            migrationBuilder.AddColumn<int>(
                name: "NationalRanking",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalRanking",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "WorldRanking",
                table: "Players",
                newName: "Ranking");
        }
    }
}
