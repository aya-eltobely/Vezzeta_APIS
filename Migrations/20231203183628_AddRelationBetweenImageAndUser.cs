using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezetaApi.Migrations
{
    public partial class AddRelationBetweenImageAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserImageId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserImageId",
                table: "Users",
                column: "UserImageId",
                unique: true,
                filter: "[UserImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_UserImageId",
                table: "Users",
                column: "UserImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_UserImageId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserImageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserImageId",
                table: "Users");
        }
    }
}
