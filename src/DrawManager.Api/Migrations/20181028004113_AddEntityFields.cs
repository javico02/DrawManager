using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Api.Migrations
{
    public partial class AddEntityFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DrawEntries",
                table: "DrawEntries");

            migrationBuilder.DropColumn(
                name: "PriceSelectionStepType",
                table: "PrizeSelectionSteps");

            migrationBuilder.AddColumn<int>(
                name: "PrizeSelectionStepType",
                table: "PrizeSelectionSteps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutedOn",
                table: "Prizes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowMultipleParticipations",
                table: "Draws",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DrawEntries",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrawEntries",
                table: "DrawEntries",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DrawEntries_DrawId",
                table: "DrawEntries",
                column: "DrawId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DrawEntries",
                table: "DrawEntries");

            migrationBuilder.DropIndex(
                name: "IX_DrawEntries_DrawId",
                table: "DrawEntries");

            migrationBuilder.DropColumn(
                name: "PrizeSelectionStepType",
                table: "PrizeSelectionSteps");

            migrationBuilder.DropColumn(
                name: "ExecutedOn",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "AllowMultipleParticipations",
                table: "Draws");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DrawEntries");

            migrationBuilder.AddColumn<int>(
                name: "PriceSelectionStepType",
                table: "PrizeSelectionSteps",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrawEntries",
                table: "DrawEntries",
                columns: new[] { "DrawId", "EntrantId" });
        }
    }
}
