using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketSpeed.Infrastructure.Migrations
{
    public partial class ImageUrlTableAndOneToManyBetweenPostAndImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostImageUrls");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "ImageUrls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImageUrls_PostId",
                table: "ImageUrls",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageUrls_Posts_PostId",
                table: "ImageUrls",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageUrls_Posts_PostId",
                table: "ImageUrls");

            migrationBuilder.DropIndex(
                name: "IX_ImageUrls_PostId",
                table: "ImageUrls");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "ImageUrls");

            migrationBuilder.CreateTable(
                name: "PostImageUrls",
                columns: table => new
                {
                    ImageUrlId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImageUrls", x => new { x.ImageUrlId, x.PostId });
                    table.ForeignKey(
                        name: "FK_PostImageUrls_ImageUrls_ImageUrlId",
                        column: x => x.ImageUrlId,
                        principalTable: "ImageUrls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostImageUrls_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImageUrls_PostId",
                table: "PostImageUrls",
                column: "PostId");
        }
    }
}
