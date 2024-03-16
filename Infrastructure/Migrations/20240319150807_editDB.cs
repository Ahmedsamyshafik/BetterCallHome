using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "royalDocuments");

            migrationBuilder.DropColumn(
                name: "VideoName",
                table: "apartmentVideos");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "apartmentImages");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "royalDocuments",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "VideoPath",
                table: "apartmentVideos",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "apartmentImages",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "royalDocuments",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "apartmentVideos",
                newName: "VideoPath");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "apartmentImages",
                newName: "ImagePath");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "royalDocuments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VideoName",
                table: "apartmentVideos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "apartmentImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
