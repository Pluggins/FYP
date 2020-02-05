using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FYP.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberCaptures",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DeletedBy = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Deleted = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    AppLoginSessionId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberCaptures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberCaptures_AppLoginSessions_AppLoginSessionId",
                        column: x => x.AppLoginSessionId,
                        principalTable: "AppLoginSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberCaptures_AppLoginSessionId",
                table: "MemberCaptures",
                column: "AppLoginSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberCaptures");
        }
    }
}
