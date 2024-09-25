using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class eighth_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("d65ffd32-905c-462a-81c9-1504de3444ec"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d546379f-6aaa-45d0-9b0c-bf3d9cb507cf"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("223722f1-b3b5-4f6d-8e6b-ec123d0c5b65"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("8fafc4fe-0f72-4ec3-9ee1-2d0cd4114b8f"), "admin@gmail.com", false, "$2a$11$IKogc9k7TCR7H6/nz0z.TObQhBPNhFUkGONQc6CzrYdyT0x0xTdsa", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("d65ffd32-905c-462a-81c9-1504de3444ec"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("d546379f-6aaa-45d0-9b0c-bf3d9cb507cf"), "admin@gmail.com", false, "$2a$11$5CyGoJHtkVo.XUC/MAGp8O.4k248twzgNZ4XUdOszE0v3aM18vZfq", 1 });
        }
    }
}
