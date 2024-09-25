using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class sixthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("2501a241-60d6-4ae4-ae79-734cffc3a4a2"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("d33f6f15-39c9-495e-9ac0-a7ce6bebf92f"), "admin@gmail.com", false, "$2a$11$xmORarkC9cr/yC0OF.ux3OZvm/5qIYD1xUIQlPVoWhqY9h0dz8MvS", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("2501a241-60d6-4ae4-ae79-734cffc3a4a2"));

          

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d33f6f15-39c9-495e-9ac0-a7ce6bebf92f"));

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("b82ee34a-771d-4877-b440-8fc047b66727"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("0e9bcfdf-7ba2-47fe-b619-a518b2e70989"), "admin@gmail.com", false, "$2a$11$vIKE04QXi.vm2GIbEjYH/uSSs1kgwrP9Uz2MwiT4G1dguKQSFgTiu", 1 });
        }
    }
}
