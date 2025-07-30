using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookShob.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    catName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    catOrder = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Author = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "catName", "catOrder", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Category 1", 1, false },
                    { 2, "Category 2", 2, false },
                    { 3, "Category 3", 3, false },
                    { 4, "Category 4", 4, false },
                    { 5, "Category 5", 5, false },
                    { 6, "Category 6", 6, false },
                    { 7, "Category 7", 7, false },
                    { 8, "Category 8", 8, false },
                    { 9, "Category 9", 9, false },
                    { 10, "Category 10", 10, false },
                    { 11, "Category 11", 11, false },
                    { 12, "Category 12", 12, false },
                    { 13, "Category 13", 13, false },
                    { 14, "Category 14", 14, false },
                    { 15, "Category 15", 15, false },
                    { 16, "Category 16", 16, false },
                    { 17, "Category 17", 17, false },
                    { 18, "Category 18", 18, false },
                    { 19, "Category 19", 19, false },
                    { 20, "Category 20", 20, false }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "BookPrice", "Title", "isDeleted" },
                values: new object[,]
                {
                    { 1, "Author 1", 1, "This is the description of Book 1", 10.0m, "Book 1", false },
                    { 2, "Author 2", 2, "This is the description of Book 2", 20.0m, "Book 2", false },
                    { 3, "Author 3", 3, "This is the description of Book 3", 30.0m, "Book 3", false },
                    { 4, "Author 4", 4, "This is the description of Book 4", 40.0m, "Book 4", false },
                    { 5, "Author 5", 5, "This is the description of Book 5", 50.0m, "Book 5", false },
                    { 6, "Author 6", 6, "This is the description of Book 6", 60.0m, "Book 6", false },
                    { 7, "Author 7", 7, "This is the description of Book 7", 70.0m, "Book 7", false },
                    { 8, "Author 8", 8, "This is the description of Book 8", 80.0m, "Book 8", false },
                    { 9, "Author 9", 9, "This is the description of Book 9", 90.0m, "Book 9", false },
                    { 10, "Author 10", 10, "This is the description of Book 10", 100.0m, "Book 10", false },
                    { 11, "Author 11", 11, "This is the description of Book 11", 110.0m, "Book 11", false },
                    { 12, "Author 12", 12, "This is the description of Book 12", 120.0m, "Book 12", false },
                    { 13, "Author 13", 13, "This is the description of Book 13", 130.0m, "Book 13", false },
                    { 14, "Author 14", 14, "This is the description of Book 14", 140.0m, "Book 14", false },
                    { 15, "Author 15", 15, "This is the description of Book 15", 150.0m, "Book 15", false },
                    { 16, "Author 16", 16, "This is the description of Book 16", 160.0m, "Book 16", false },
                    { 17, "Author 17", 17, "This is the description of Book 17", 170.0m, "Book 17", false },
                    { 18, "Author 18", 18, "This is the description of Book 18", 180.0m, "Book 18", false },
                    { 19, "Author 19", 19, "This is the description of Book 19", 190.0m, "Book 19", false },
                    { 20, "Author 20", 20, "This is the description of Book 20", 200.0m, "Book 20", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_catName",
                table: "Categories",
                column: "catName");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
