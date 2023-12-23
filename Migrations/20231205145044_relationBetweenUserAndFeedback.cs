using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezetaApi.Migrations
{
    public partial class relationBetweenUserAndFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Feedbacks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AppUserId",
                table: "Feedbacks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_AppUserId",
                table: "Feedbacks",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_AppUserId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_AppUserId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Feedbacks");
        }
    }
}
