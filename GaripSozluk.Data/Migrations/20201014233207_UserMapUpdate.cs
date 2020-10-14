using Microsoft.EntityFrameworkCore.Migrations;

namespace GaripSozluk.Data.Migrations
{
    public partial class UserMapUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUsers_AspNetUsers_UserId",
                table: "BlockedUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlockedUsers_UserId",
                table: "BlockedUsers");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUsers_BlockedUserId",
                table: "BlockedUsers",
                column: "BlockedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUsers_AspNetUsers_BlockedUserId",
                table: "BlockedUsers",
                column: "BlockedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockedUsers_AspNetUsers_BlockedUserId",
                table: "BlockedUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlockedUsers_BlockedUserId",
                table: "BlockedUsers");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedUsers_UserId",
                table: "BlockedUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockedUsers_AspNetUsers_UserId",
                table: "BlockedUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
