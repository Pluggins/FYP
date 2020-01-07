using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                table: "Menus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDesc",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDesc",
                table: "MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "LongDesc",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ShortDesc",
                table: "MenuItems");
        }
    }
}
