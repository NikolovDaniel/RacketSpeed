using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacketSpeed.Infrastructure.Migrations
{
    public partial class AddedImageUrlAndPostImageUrlDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImageUrl_ImageUrl_ImageUrlId",
                table: "PostImageUrl");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImageUrl_Posts_PostId",
                table: "PostImageUrl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImageUrl",
                table: "PostImageUrl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageUrl",
                table: "ImageUrl");

            migrationBuilder.RenameTable(
                name: "PostImageUrl",
                newName: "PostImageUrls");

            migrationBuilder.RenameTable(
                name: "ImageUrl",
                newName: "ImageUrls");

            migrationBuilder.RenameIndex(
                name: "IX_PostImageUrl_PostId",
                table: "PostImageUrls",
                newName: "IX_PostImageUrls_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImageUrls",
                table: "PostImageUrls",
                columns: new[] { "ImageUrlId", "PostId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageUrls",
                table: "ImageUrls",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostImageUrls_ImageUrls_ImageUrlId",
                table: "PostImageUrls",
                column: "ImageUrlId",
                principalTable: "ImageUrls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImageUrls_Posts_PostId",
                table: "PostImageUrls",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostImageUrls_ImageUrls_ImageUrlId",
                table: "PostImageUrls");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImageUrls_Posts_PostId",
                table: "PostImageUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImageUrls",
                table: "PostImageUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageUrls",
                table: "ImageUrls");

            migrationBuilder.RenameTable(
                name: "PostImageUrls",
                newName: "PostImageUrl");

            migrationBuilder.RenameTable(
                name: "ImageUrls",
                newName: "ImageUrl");

            migrationBuilder.RenameIndex(
                name: "IX_PostImageUrls_PostId",
                table: "PostImageUrl",
                newName: "IX_PostImageUrl_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImageUrl",
                table: "PostImageUrl",
                columns: new[] { "ImageUrlId", "PostId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageUrl",
                table: "ImageUrl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostImageUrl_ImageUrl_ImageUrlId",
                table: "PostImageUrl",
                column: "ImageUrlId",
                principalTable: "ImageUrl",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImageUrl_Posts_PostId",
                table: "PostImageUrl",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
