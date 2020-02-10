using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MethodId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Quantity",
                table: "PaymentItems",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "PaymentItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
