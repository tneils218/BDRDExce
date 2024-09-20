using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDRDExce.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Submissions",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_CourseId",
                table: "Submissions",
                newName: "IX_Submissions_ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Exams_ExamId",
                table: "Submissions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Exams_ExamId",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                table: "Submissions",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Submissions_ExamId",
                table: "Submissions",
                newName: "IX_Submissions_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Courses_CourseId",
                table: "Submissions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
