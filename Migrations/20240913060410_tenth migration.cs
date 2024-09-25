using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class tenthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("853c2e69-a88c-4449-8878-27db782b3654"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("a683dace-56e8-4c2e-b6b6-40ec93747631"), "admin@gmail.com", false, "$2a$11$ZBW33enHkuLVRQrI5LKbduO8aPxL6TGGzqU9ZgiqgTDcFuHHdGvEG", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("853c2e69-a88c-4449-8878-27db782b3654"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a683dace-56e8-4c2e-b6b6-40ec93747631"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("0833b146-b753-4eb2-944f-78eaf87d1f7b"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("f66fd7b7-3eff-4878-af5d-769c64ccf3b0"), "admin@gmail.com", false, "$2a$11$UBFkfS6jfFp54iqkiwb2QuDt/cgTKyWTM2fDUUJMOq2vnAVcb1GWy", 1 });
        }
    }
}
