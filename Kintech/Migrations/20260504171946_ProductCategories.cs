using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kintech.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, "Phone Accessories", null },
                    { 10, "Audio", null },
                    { 20, "Wearables", null },
                    { 30, "Computing", null },
                    { 40, "Gaming", null },
                    { 50, "Smart Home", null },
                    { 60, "Photography", null },
                    { 70, "Power & Connectivity", null },
                    { 80, "Storage & Protection", null },
                    { 90, "Deals", null },
                    { 2, "Cases & Covers", 1 },
                    { 3, "Screen Protectors", 1 },
                    { 4, "Chargers & Cables", 1 },
                    { 5, "Power Banks", 1 },
                    { 6, "Phone Stands & Holders", 1 },
                    { 11, "Wireless Earbuds", 10 },
                    { 12, "Headphones", 10 },
                    { 13, "Bluetooth Speakers", 10 },
                    { 21, "Smartwatches", 20 },
                    { 22, "Fitness Trackers", 20 },
                    { 31, "Laptops & Tablets", 30 },
                    { 32, "Keyboards & Mice", 30 },
                    { 33, "USB Hubs & Docks", 30 },
                    { 41, "Gaming Controllers", 40 },
                    { 42, "Gaming Headsets", 40 },
                    { 51, "Smart Lighting", 50 },
                    { 52, "Security Cameras", 50 },
                    { 61, "Tripods", 60 },
                    { 62, "Ring Lights", 60 },
                    { 71, "Wireless Chargers", 70 },
                    { 81, "SSD & Hard Drives", 80 },
                    { 91, "Best Sellers", 90 },
                    { 92, "New Arrivals", 90 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");
        }
    }
}
