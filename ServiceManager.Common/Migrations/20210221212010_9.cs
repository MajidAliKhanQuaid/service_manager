using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ServiceManager.Common.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceCommandResponse");

            migrationBuilder.DropTable(
                name: "ServiceRequestCommand");

            migrationBuilder.CreateTable(
                name: "CommandRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastProcessedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandRequest_SystemService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SystemService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommandResponse",
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
                    table.PrimaryKey("PK_CommandResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommandResponse_CommandRequest_ServiceRequestCommandId",
                        column: x => x.ServiceRequestCommandId,
                        principalTable: "CommandRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommandRequest_ServiceId",
                table: "CommandRequest",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CommandResponse_ServiceRequestCommandId",
                table: "CommandResponse",
                column: "ServiceRequestCommandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandResponse");

            migrationBuilder.DropTable(
                name: "CommandRequest");

            migrationBuilder.CreateTable(
                name: "ServiceRequestCommand",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastProcessedUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceRequestCommand_SystemService_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "SystemService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceCommandResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Command = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConsoleMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ServiceRequestCommandId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestCommand_ServiceId",
                table: "ServiceRequestCommand",
                column: "ServiceId");
        }
    }
}
