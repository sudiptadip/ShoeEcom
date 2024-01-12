using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAddress_AspNetUsers_ApplicationUserId",
                table: "OrderAddress");

            migrationBuilder.DropIndex(
                name: "IX_OrderAddress_ApplicationUserId",
                table: "OrderAddress");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "OrderAddress",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderAddressId",
                table: "OrderItems",
                column: "OrderAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_OrderAddress_OrderAddressId",
                table: "OrderItems",
                column: "OrderAddressId",
                principalTable: "OrderAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_OrderAddress_OrderAddressId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderAddressId",
                table: "OrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "OrderAddress",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddress_ApplicationUserId",
                table: "OrderAddress",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAddress_AspNetUsers_ApplicationUserId",
                table: "OrderAddress",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
