using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class merak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apartmentsComments_Apartment_ApartmentId",
                table: "apartmentsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_apartmentsComments_AspNetUsers_UserId",
                table: "apartmentsComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_apartmentsComments",
                table: "apartmentsComments");

            migrationBuilder.RenameTable(
                name: "apartmentsComments",
                newName: "userapartmentsComments");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "Apartment",
                newName: "Likes");

            migrationBuilder.RenameIndex(
                name: "IX_apartmentsComments_UserId",
                table: "userapartmentsComments",
                newName: "IX_userapartmentsComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_apartmentsComments_ApartmentId",
                table: "userapartmentsComments",
                newName: "IX_userapartmentsComments_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userapartmentsComments",
                table: "userapartmentsComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments",
                column: "ApartmentId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userapartmentsComments_AspNetUsers_UserId",
                table: "userapartmentsComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userapartmentsComments_Apartment_ApartmentId",
                table: "userapartmentsComments");

            migrationBuilder.DropForeignKey(
                name: "FK_userapartmentsComments_AspNetUsers_UserId",
                table: "userapartmentsComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userapartmentsComments",
                table: "userapartmentsComments");

            migrationBuilder.RenameTable(
                name: "userapartmentsComments",
                newName: "apartmentsComments");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Apartment",
                newName: "Views");

            migrationBuilder.RenameIndex(
                name: "IX_userapartmentsComments_UserId",
                table: "apartmentsComments",
                newName: "IX_apartmentsComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userapartmentsComments_ApartmentId",
                table: "apartmentsComments",
                newName: "IX_apartmentsComments_ApartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_apartmentsComments",
                table: "apartmentsComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_apartmentsComments_Apartment_ApartmentId",
                table: "apartmentsComments",
                column: "ApartmentId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_apartmentsComments_AspNetUsers_UserId",
                table: "apartmentsComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
