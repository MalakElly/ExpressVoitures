using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionVoituresExpress.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Cars",
                newName: "Trim");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Cars",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Cars",
                newName: "FixingDetails");

            migrationBuilder.RenameColumn(
                name: "DateAdded",
                table: "Cars",
                newName: "ToSellDate");

            migrationBuilder.AlterColumn<int>(
                name: "km",
                table: "Cars",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BuyingDate",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "BuyingPrice",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CodeVIN",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FixingPrice",
                table: "Cars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SellingDate",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BuyingDate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "BuyingPrice",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CodeVIN",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "FixingPrice",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "SellingDate",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "Trim",
                table: "Cars",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ToSellDate",
                table: "Cars",
                newName: "DateAdded");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Cars",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "FixingDetails",
                table: "Cars",
                newName: "Description");

            migrationBuilder.AlterColumn<int>(
                name: "km",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
