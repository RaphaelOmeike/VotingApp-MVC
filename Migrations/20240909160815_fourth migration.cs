using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class fourthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80c38d39-88d6-40c4-818c-5d20bb9b793d"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("44770499-70f2-4f7d-8cc5-135af36a666e"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("84173fa6-d4f1-45b0-9f55-c0c6e6256c66"), "admin@gmail.com", false, "$2a$11$SJz3VEBTi7IV7wZ7XSqLJ.G9o3bmISKdGLimzL2cR/V0KF5oViIhG", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("44770499-70f2-4f7d-8cc5-135af36a666e"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("84173fa6-d4f1-45b0-9f55-c0c6e6256c66"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("80c38d39-88d6-40c4-818c-5d20bb9b793d"), "admin@gmail.com", false, "$2a$11$UPtVroZ47htfruY4Mwe7KuHq5d2OoKLHkrXE9ypuMHaCZPQD57bFi", 1 });
        }
    }
}
