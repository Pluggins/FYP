using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "MenuItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitingTime",
                table: "MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "WaitingTime",
                table: "MenuItems");
        }
    }
}
