using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDO_List.DAL.Data.Migrations
{
    public partial class AddCascadeOnTaksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
