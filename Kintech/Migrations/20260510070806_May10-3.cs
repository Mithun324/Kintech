using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kintech.Migrations
{
    /// <inheritdoc />
    public partial class May103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliverySlotId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeliverySlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeSlot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliverySlots", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DeliverySlots",
                columns: new[] { "Id", "IsActive", "TimeSlot" },
                values: new object[,]
                {
                    { 1, true, "9 AM - 12 PM" },
                    { 2, true, "12 PM - 3 PM" },
                    { 3, true, "3 PM - 6 PM" },
                    { 4, true, "6 PM - 9 PM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliverySlotId",
                table: "Orders",
                column: "DeliverySlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliverySlots_DeliverySlotId",
                table: "Orders",
                column: "DeliverySlotId",
                principalTable: "DeliverySlots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliverySlots_DeliverySlotId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "DeliverySlots");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DeliverySlotId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliverySlotId",
                table: "Orders");
        }
    }
}
