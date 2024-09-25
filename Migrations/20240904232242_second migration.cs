using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class secondmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("92c9bc80-a640-40e2-8037-afc6a7ef0964"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDisqualified",
                table: "CandidatePositions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("16ac33fb-0ffc-496a-838e-8a313594245d"), "admin@gmail.com", false, "$2a$11$BppJQ6Q.HPOh.I04u6mi1.1XD53Vqrj9SftVyScyEGlugBco1r7vC", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("16ac33fb-0ffc-496a-838e-8a313594245d"));

            migrationBuilder.DropColumn(
                name: "IsDisqualified",
                table: "CandidatePositions");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("92c9bc80-a640-40e2-8037-afc6a7ef0964"), "admin@gmail.com", false, "$2a$11$kg9kJS.3QIYnLWN64X3GXOGu7z9qG1rPJUKsut/48OcF71YYaoJGO", 1 });
        }
    }
}
