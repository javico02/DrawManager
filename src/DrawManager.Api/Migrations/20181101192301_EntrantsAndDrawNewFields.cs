using Microsoft.EntityFrameworkCore.Migrations;

namespace DrawManager.Api.Migrations
{
    public partial class EntrantsAndDrawNewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchOffice",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Office",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubDepartment",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subsidiary",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Entrants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Draws",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchOffice",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "Office",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "SubDepartment",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "Subsidiary",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Entrants");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Draws");
        }
    }
}
