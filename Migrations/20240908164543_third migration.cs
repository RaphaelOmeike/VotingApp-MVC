using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VotingApp.Migrations
{
    /// <inheritdoc />
    public partial class thirdmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Rules_RuleId",
                table: "Positions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("16ac33fb-0ffc-496a-838e-8a313594245d"));

            migrationBuilder.DropColumn(
                name: "Program",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "Rules");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCasted",
                table: "VoteCastingInfos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "CanVote",
                table: "Students",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Students",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Rules",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "RuleId",
                table: "Positions",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Positions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RuleId",
                table: "Elections",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DisqualifierId",
                table: "CandidatePositions",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VotesNo",
                table: "CandidatePositions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("80c38d39-88d6-40c4-818c-5d20bb9b793d"), "admin@gmail.com", false, "$2a$11$UPtVroZ47htfruY4Mwe7KuHq5d2OoKLHkrXE9ypuMHaCZPQD57bFi", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseId",
                table: "Students",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_CourseId",
                table: "Rules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Elections_RuleId",
                table: "Elections",
                column: "RuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elections_Rules_RuleId",
                table: "Elections",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Rules_RuleId",
                table: "Positions",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Courses_CourseId",
                table: "Rules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elections_Rules_RuleId",
                table: "Elections");

            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Rules_RuleId",
                table: "Positions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Courses_CourseId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Students_CourseId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Rules_CourseId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Elections_RuleId",
                table: "Elections");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("80c38d39-88d6-40c4-818c-5d20bb9b793d"));

            migrationBuilder.DropColumn(
                name: "DateCasted",
                table: "VoteCastingInfos");

            migrationBuilder.DropColumn(
                name: "CanVote",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "DisqualifierId",
                table: "CandidatePositions");

            migrationBuilder.DropColumn(
                name: "VotesNo",
                table: "CandidatePositions");

            migrationBuilder.AddColumn<string>(
                name: "Program",
                table: "Students",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Program",
                table: "Rules",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "RuleId",
                table: "Positions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsDeleted", "PasswordHash", "Role" },
                values: new object[] { new Guid("16ac33fb-0ffc-496a-838e-8a313594245d"), "admin@gmail.com", false, "$2a$11$BppJQ6Q.HPOh.I04u6mi1.1XD53Vqrj9SftVyScyEGlugBco1r7vC", 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Rules_RuleId",
                table: "Positions",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "Id");
        }
    }
}
