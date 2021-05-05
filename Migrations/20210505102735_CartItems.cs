using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStoreProject.Migrations
{
    public partial class CartItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProductDTO_ProductDTOId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "ProductDTO");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductDTOId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ProductDTOId",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ShoppingCarts",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductDTOId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ProductName = table.Column<string>(type: "longtext", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<double>(type: "double", nullable: true),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDTO_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductDTOId",
                table: "Comments",
                column: "ProductDTOId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDTO_ShoppingCartId",
                table: "ProductDTO",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProductDTO_ProductDTOId",
                table: "Comments",
                column: "ProductDTOId",
                principalTable: "ProductDTO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
