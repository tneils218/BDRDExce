using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDRDExce.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Medias_MediaId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_MediaId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Courses",
                newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "CourseMedias",
                columns: table => new
                {
                    MediaId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<int>(type: "integer", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMedias");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Courses",
                newName: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MediaId",
                table: "Courses",
                column: "MediaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Medias_MediaId",
                table: "Courses",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
