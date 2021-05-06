using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineStoreProject.Migrations
{
    public partial class SomeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "isApproved",
                table: "Comments",
                newName: "IsApproved");

            migrationBuilder.RenameColumn(
                name: "commentId",
                table: "Comments",
                newName: "CommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "IsApproved",
                table: "Comments",
                newName: "isApproved");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "commentId");
        }
    }
}
