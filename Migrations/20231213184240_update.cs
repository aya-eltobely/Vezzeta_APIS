using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezetaApi.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_OppintmentId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_OppintmentId",
                table: "Bookings",
                column: "OppintmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_OppintmentId",
                table: "Bookings");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_OppintmentId",
                table: "Bookings",
                column: "OppintmentId",
                unique: true);
        }
    }
}
