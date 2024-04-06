using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class whyas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersApartments_Apartment_ApartmentID",
                table: "UsersApartments");

            migrationBuilder.AlterColumn<int>(
                name: "ApartmentID",
                table: "UsersApartments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersApartments_Apartment_ApartmentID",
                table: "UsersApartments",
                column: "ApartmentID",
                principalTable: "Apartment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersApartments_Apartment_ApartmentID",
                table: "UsersApartments");

            migrationBuilder.AlterColumn<int>(
                name: "ApartmentID",
                table: "UsersApartments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersApartments_Apartment_ApartmentID",
                table: "UsersApartments",
                column: "ApartmentID",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
