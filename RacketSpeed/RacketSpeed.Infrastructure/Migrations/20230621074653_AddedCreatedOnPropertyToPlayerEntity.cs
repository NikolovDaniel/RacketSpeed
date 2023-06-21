using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketSpeed.Infrastructure.Migrations
{
    public partial class AddedCreatedOnPropertyToPlayerEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Players",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Players");
        }
    }
}
