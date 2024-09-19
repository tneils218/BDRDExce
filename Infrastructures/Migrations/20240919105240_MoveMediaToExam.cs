using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDRDExce.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class MoveMediaToExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMedias");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Medias",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExamMedias",
                columns: table => new
                {
                    MediaId = table.Column<string>(type: "text", nullable: false),
                    ExamId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamMedias", x => new { x.ExamId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_ExamMedias_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamMedias_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medias_CourseId",
                table: "Medias",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamMedias_MediaId",
                table: "ExamMedias",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medias_Courses_CourseId",
                table: "Medias",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medias_Courses_CourseId",
                table: "Medias");

            migrationBuilder.DropTable(
                name: "ExamMedias");

            migrationBuilder.DropIndex(
                name: "IX_Medias_CourseId",
                table: "Medias");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Medias");

            migrationBuilder.CreateTable(
                name: "CourseMedias",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "integer", nullable: false),
                    MediaId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMedias", x => new { x.CourseId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_CourseMedias_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMedias_Medias_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Medias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMedias_MediaId",
                table: "CourseMedias",
                column: "MediaId");
        }
    }
}
