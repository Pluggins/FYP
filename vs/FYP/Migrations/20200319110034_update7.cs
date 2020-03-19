using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PaymentItems");

            migrationBuilder.AddColumn<double>(
                name: "QuantityPaid",
                table: "OrderItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityPaid",
                table: "OrderItems");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PaymentItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
