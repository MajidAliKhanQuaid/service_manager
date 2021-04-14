using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceManager.Common.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SystemService_ProjectId",
                table: "SystemService",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemService_Project_ProjectId",
                table: "SystemService",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemService_Project_ProjectId",
                table: "SystemService");

            migrationBuilder.DropIndex(
                name: "IX_SystemService_ProjectId",
                table: "SystemService");
        }
    }
}
