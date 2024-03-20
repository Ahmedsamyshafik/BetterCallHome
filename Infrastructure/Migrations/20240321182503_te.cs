using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class te : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment");

            migrationBuilder.DropForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments",
                column: "ApartmentId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment");

            migrationBuilder.DropForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments",
                column: "ApartmentId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
