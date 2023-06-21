using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketSpeed.Infrastructure.Migrations
{
    public partial class AddedBirthPlacePropertyToPlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BirthPlace",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthPlace",
                table: "Players");
        }
    }
}
