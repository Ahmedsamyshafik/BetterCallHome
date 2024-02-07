using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "2", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "3", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9854f42d-f5e9-4e63-82d8-2f4ab9926767");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bb9aca7-a66d-4fa8-a1f3-0bee0c372602");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Age", "College", "ConcurrencyStamp", "Email", "EmailConfirmed", "Gender", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "University", "UserApartmentId", "UserName" },
                values: new object[] { "9854f42d-f5e9-4e63-82d8-2f4ab9926767", 0, 22, "CS", "e7aa6ff6-537b-4844-897d-d5e4597c8a61", "Admin@gmail.com", false, "M", false, null, null, null, null, "01095385375", false, "9c45bfda-9d63-4fae-bd6d-b0891564691a", false, "DU", 0, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "1", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" },
                    { "2", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" },
                    { "3", "9854f42d-f5e9-4e63-82d8-2f4ab9926767" }
                });
        }
    }
}
