using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class fifthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("b82ee34a-771d-4877-b440-8fc047b66727"), "represents all the courses", false, "all courses" });

            
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("0e9bcfdf-7ba2-47fe-b619-a518b2e70989"), "admin@gmail.com", false, "$2a$11$vIKE04QXi.vm2GIbEjYH/uSSs1kgwrP9Uz2MwiT4G1dguKQSFgTiu", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("b82ee34a-771d-4877-b440-8fc047b66727"));


            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0e9bcfdf-7ba2-47fe-b619-a518b2e70989"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("44770499-70f2-4f7d-8cc5-135af36a666e"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("84173fa6-d4f1-45b0-9f55-c0c6e6256c66"), "admin@gmail.com", false, "$2a$11$SJz3VEBTi7IV7wZ7XSqLJ.G9o3bmISKdGLimzL2cR/V0KF5oViIhG", 1 });
        }
    }
}
