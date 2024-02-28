using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaratCount.Migrations
{
    public partial class Clients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GstInDetails_AspNetUsers_UserId",
                table: "GstInDetails");

            migrationBuilder.DropIndex(
                name: "IX_GstInDetails_UserId",
                table: "GstInDetails");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GstInDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GstInDetailId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    GstInDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_GstInDetails_GstInDetailId",
                        column: x => x.GstInDetailId,
                        principalTable: "GstInDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GstInDetailId",
                table: "AspNetUsers",
                column: "GstInDetailId",
                unique: true,
                filter: "[GstInDetailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_GstInDetailId",
                table: "Clients",
                column: "GstInDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_GstInDetails_GstInDetailId",
                table: "AspNetUsers",
                column: "GstInDetailId",
                principalTable: "GstInDetails",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_GstInDetails_GstInDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GstInDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GstInDetailId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GstInDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GstInDetails_UserId",
                table: "GstInDetails",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_GstInDetails_AspNetUsers_UserId",
                table: "GstInDetails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
