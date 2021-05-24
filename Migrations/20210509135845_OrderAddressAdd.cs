using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStoreProject.Migrations
{
    public partial class OrderAddressAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Customers",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldDefaultValue: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Customers",
                type: "longtext",
                nullable: false,
                defaultValue: "Customer",
                oldClrType: typeof(string),
                oldType: "longtext");
        }
    }
}
