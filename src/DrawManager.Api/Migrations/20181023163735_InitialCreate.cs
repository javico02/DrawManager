using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draws",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProgrammedFor = table.Column<DateTime>(nullable: false),
                    ExecutedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draws", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entrants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    Hash = table.Column<byte[]>(nullable: true),
                    Salt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DrawId = table.Column<int>(nullable: false),
                    AttemptsUntilChooseWinner = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prizes_Draws_DrawId",
                        column: x => x.DrawId,
                        principalTable: "Draws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrawEntries",
                columns: table => new
                {
                    DrawId = table.Column<int>(nullable: false),
                    EntrantId = table.Column<int>(nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrawEntries", x => new { x.DrawId, x.EntrantId });
                    table.ForeignKey(
                        name: "FK_DrawEntries_Draws_DrawId",
                        column: x => x.DrawId,
                        principalTable: "Draws",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrawEntries_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrizeSelectionSteps",
                columns: table => new
                {
                    PrizeId = table.Column<int>(nullable: false),
                    EntrantId = table.Column<int>(nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    PriceSelectionStepType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrizeSelectionSteps", x => new { x.PrizeId, x.EntrantId });
                    table.ForeignKey(
                        name: "FK_PrizeSelectionSteps_Entrants_EntrantId",
                        column: x => x.EntrantId,
                        principalTable: "Entrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrizeSelectionSteps_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrawEntries_EntrantId",
                table: "DrawEntries",
                column: "EntrantId");

            migrationBuilder.CreateIndex(
                name: "IX_Prizes_DrawId",
                table: "Prizes",
                column: "DrawId");

            migrationBuilder.CreateIndex(
                name: "IX_PrizeSelectionSteps_EntrantId",
                table: "PrizeSelectionSteps",
                column: "EntrantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrawEntries");

            migrationBuilder.DropTable(
                name: "PrizeSelectionSteps");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Entrants");

            migrationBuilder.DropTable(
                name: "Prizes");

            migrationBuilder.DropTable(
                name: "Draws");
        }
    }
}
