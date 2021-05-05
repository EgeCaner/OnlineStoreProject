using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStoreProject.Migrations
{
    public partial class CustCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CartId",
                table: "Customers",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ShoppingCarts_CartId",
                table: "Customers",
                column: "CartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ShoppingCarts_CartId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CartId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "Customers");
        }
    }
}
