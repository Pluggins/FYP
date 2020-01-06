using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Users_UserId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Vendors",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_OwnerId",
                table: "Vendors",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Users_OwnerId",
                table: "Vendors",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Users_OwnerId",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_OwnerId",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Vendors");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Vendors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_UserId",
                table: "Vendors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Users_UserId",
                table: "Vendors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
