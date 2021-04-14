using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceManager.Common.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceCommandResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceRequestCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsoleMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceCommandResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceCommandResponse_ServiceRequestCommand_ServiceRequestCommandId",
                        column: x => x.ServiceRequestCommandId,
                        principalTable: "ServiceRequestCommand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCommandResponse_ServiceRequestCommandId",
                table: "ServiceCommandResponse",
                column: "ServiceRequestCommandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceCommandResponse");
        }
    }
}
