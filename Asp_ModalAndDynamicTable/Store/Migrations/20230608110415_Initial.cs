using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.CheckConstraint("CK_Articles_Price", "Price > 0");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderArticles",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ArticleQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderArticles", x => new { x.ArticleId, x.OrderId });
                    table.CheckConstraint("CK_Articles_Price1", "ArticleQuantity > 0");
                    table.ForeignKey(
                        name: "FK_OrderArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderArticles_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Труба", 50.5m },
                    { 2, "Корпус", 199m },
                    { 3, "Набор креплений", 23.78m },
                    { 4, "Утеплитель", 10.11m }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                column: "Id",
                values: new object[]
                {
                    1,
                    2
                });

            migrationBuilder.InsertData(
                table: "OrderArticles",
                columns: new[] { "ArticleId", "OrderId", "ArticleQuantity" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 1, 1 },
                    { 3, 2, 3 },
                    { 4, 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderArticles_OrderId",
                table: "OrderArticles",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderArticles");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
