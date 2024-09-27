using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BDRDExce.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MediaId",
                table: "AspNetUsers",
                column: "MediaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Medias_MediaId",
                table: "AspNetUsers",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Medias_MediaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MediaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "AspNetUsers");
        }
    }
}
