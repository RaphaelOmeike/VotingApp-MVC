using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class seventh_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("2501a241-60d6-4ae4-ae79-734cffc3a4a2"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d33f6f15-39c9-495e-9ac0-a7ce6bebf92f"));

            migrationBuilder.AddColumn<bool>(
                name: "Winner",
                table: "CandidatePositions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("d65ffd32-905c-462a-81c9-1504de3444ec"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("d546379f-6aaa-45d0-9b0c-bf3d9cb507cf"), "admin@gmail.com", false, "$2a$11$5CyGoJHtkVo.XUC/MAGp8O.4k248twzgNZ4XUdOszE0v3aM18vZfq", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("d65ffd32-905c-462a-81c9-1504de3444ec"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d546379f-6aaa-45d0-9b0c-bf3d9cb507cf"));

            migrationBuilder.DropColumn(
                name: "Winner",
                table: "CandidatePositions");

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[] { new Guid("2501a241-60d6-4ae4-ae79-734cffc3a4a2"), "represents all the courses", false, "all courses" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("d33f6f15-39c9-495e-9ac0-a7ce6bebf92f"), "admin@gmail.com", false, "$2a$11$xmORarkC9cr/yC0OF.ux3OZvm/5qIYD1xUIQlPVoWhqY9h0dz8MvS", 1 });
        }
    }
}
