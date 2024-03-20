using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class cas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartment_AspNetUsers_OwnerId",
                table: "Apartment",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
