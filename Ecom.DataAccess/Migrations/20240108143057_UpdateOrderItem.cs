using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecom.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Categorys_CategoryId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_OrderItems_OrderItemId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_OrderItemId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "OrderItems",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "OrderItems",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_CategoryId",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderTime",
                table: "OrderItems",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Products_ProductId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItems",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "OrderItems",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                newName: "IX_OrderItems_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "ProductImages",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderTime",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_OrderItemId",
                table: "ProductImages",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Categorys_CategoryId",
                table: "OrderItems",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_OrderItems_OrderItemId",
                table: "ProductImages",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id");
        }
    }
}
