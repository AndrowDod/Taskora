using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDO_List.DAL.Data.Migrations
{
    public partial class AddUserIdToProjectTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_userId",
                table: "Projects",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_userId",
                table: "Projects",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_userId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_userId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Projects");
        }
    }
}
