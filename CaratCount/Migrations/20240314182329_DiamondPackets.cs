using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaratCount.Migrations
{
    public partial class DiamondPackets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiamondPackets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaratWeight = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Clarity = table.Column<int>(type: "int", nullable: false),
                    Cut = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<int>(type: "int", nullable: false),
                    NumberOfDiamond = table.Column<int>(type: "int", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiamondPackets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiamondPackets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiamondPackets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPackets_ClientId",
                table: "DiamondPackets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_DiamondPackets_UserId",
                table: "DiamondPackets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiamondPackets");
        }
    }
}
