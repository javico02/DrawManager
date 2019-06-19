using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Api.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Entrants_Id",
                table: "Entrants",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DrawEntries_Id",
                table: "DrawEntries",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Entrants_Id",
                table: "Entrants");

            migrationBuilder.DropIndex(
                name: "IX_DrawEntries_Id",
                table: "DrawEntries");
        }
    }
}
