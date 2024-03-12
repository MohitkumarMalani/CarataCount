using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaratCount.Migrations
{
    public partial class DiamondPacketProcesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiamondPacketProcesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalCaratWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DiamondPacketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiamondPacketProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiamondPacketProcesses_DiamondPackets_DiamondPacketId",
                        column: x => x.DiamondPacketId,
                        principalTable: "DiamondPackets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiamondPacketProcesses_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiamondPacketProcesses_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiamondPacketProcesses_ProcessPrices_ProcessPriceId",
                        column: x => x.ProcessPriceId,
                        principalTable: "ProcessPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPacketProcesses_DiamondPacketId",
                table: "DiamondPacketProcesses",
                column: "DiamondPacketId");

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPacketProcesses_EmployeeId",
                table: "DiamondPacketProcesses",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPacketProcesses_ProcessId",
                table: "DiamondPacketProcesses",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPacketProcesses_ProcessPriceId",
                table: "DiamondPacketProcesses",
                column: "ProcessPriceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiamondPacketProcesses");
        }
    }
}
