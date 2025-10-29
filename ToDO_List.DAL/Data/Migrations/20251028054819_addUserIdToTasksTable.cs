using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDO_List.DAL.Data.Migrations
{
    public partial class addUserIdToTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Tasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_userId",
                table: "Tasks",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_userId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Tasks");
        }
    }
}
