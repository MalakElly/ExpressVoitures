using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionVoituresExpress.Migrations
{
    /// <inheritdoc />
    public partial class CorrectionModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToSellDate",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Transactions",
                newName: "BuyingDate");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "Transactions",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SellingDate",
                table: "Transactions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SellingDate",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "BuyingDate",
                table: "Transactions",
                newName: "date");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "Transactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToSellDate",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
