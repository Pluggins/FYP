using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentItems_MenuItems_MenuItemId",
                table: "PaymentItems");

            migrationBuilder.DropIndex(
                name: "IX_PaymentItems_MenuItemId",
                table: "PaymentItems");

            migrationBuilder.DropColumn(
                name: "MenuItemId",
                table: "PaymentItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MenuItemId",
                table: "PaymentItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentItems_MenuItemId",
                table: "PaymentItems",
                column: "MenuItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentItems_MenuItems_MenuItemId",
                table: "PaymentItems",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
