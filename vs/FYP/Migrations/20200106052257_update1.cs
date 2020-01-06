using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLoginSessions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLoginSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLoginSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLoginSessions_UserId",
                table: "AppLoginSessions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLoginSessions");
        }
    }
}
