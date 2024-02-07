using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1", null, "User", "USER" },
                    { "2", null, "Owner", "OWNER" },
                    { "3", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "College", "ConcurrencyStamp", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "University", "UserApartmentId", "UserName" },
                values: new object[] { "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602", 0, 22, "CS", "114cac84-f463-43ec-b89f-796f5f9d9ae9", "Admin@gmail.com", false, "M", false, null, null, null, "AQAAAAIAAYagAAAAEJIZKnFibojRefvd6VoN9gypLpgr3la3C6UL1VypCcJ3qlW9ZLevr3pwz1scpGcrjA==", "01095385375", false, "83765555-d86d-4332-a5da-cbd966125a11", false, "DU", 0, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" },
                    { "2", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" },
                    { "3", "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602" }
                });
        }
    }
}
