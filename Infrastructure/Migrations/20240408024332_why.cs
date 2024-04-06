using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class why : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Apartment_CurrentLivingInId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersApartments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApartmentsID = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersApartments", x => x.id);
                    table.ForeignKey(
                        name: "FK_UsersApartments_Apartment_ApartmentID",
                        column: x => x.ApartmentID,
                        principalTable: "Apartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserID",
                table: "AspNetUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersApartments_ApartmentID",
                table: "UsersApartments",
                column: "ApartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersApartments_CurrentLivingInId",
                table: "AspNetUsers",
                column: "CurrentLivingInId",
                principalTable: "UsersApartments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersApartments_UserID",
                table: "AspNetUsers",
                column: "UserID",
                principalTable: "UsersApartments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersApartments_CurrentLivingInId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersApartments_UserID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UsersApartments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Apartment_CurrentLivingInId",
                table: "AspNetUsers",
                column: "CurrentLivingInId",
                principalTable: "Apartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
