using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class ninthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("223722f1-b3b5-4f6d-8e6b-ec123d0c5b65"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8fafc4fe-0f72-4ec3-9ee1-2d0cd4114b8f"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("0833b146-b753-4eb2-944f-78eaf87d1f7b"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("f66fd7b7-3eff-4878-af5d-769c64ccf3b0"), "admin@gmail.com", false, "$2a$11$UBFkfS6jfFp54iqkiwb2QuDt/cgTKyWTM2fDUUJMOq2vnAVcb1GWy", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("0833b146-b753-4eb2-944f-78eaf87d1f7b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f66fd7b7-3eff-4878-af5d-769c64ccf3b0"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("223722f1-b3b5-4f6d-8e6b-ec123d0c5b65"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("8fafc4fe-0f72-4ec3-9ee1-2d0cd4114b8f"), "admin@gmail.com", false, "$2a$11$IKogc9k7TCR7H6/nz0z.TObQhBPNhFUkGONQc6CzrYdyT0x0xTdsa", 1 });
        }
    }
}
