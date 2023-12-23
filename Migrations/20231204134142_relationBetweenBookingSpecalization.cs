using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezetaApi.Migrations
{
    public partial class relationBetweenBookingSpecalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SpecializationId",
                table: "Bookings",
                column: "SpecializationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Specializations_SpecializationId",
                table: "Bookings",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Specializations_SpecializationId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SpecializationId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "Bookings");
        }
    }
}
