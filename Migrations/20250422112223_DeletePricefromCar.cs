using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionVoituresExpress.Migrations
{
    /// <inheritdoc />
    public partial class DeletePricefromCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "CodeVIN",
                table: "Cars",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(17)",
                oldMaxLength: 17);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CodeVIN",
                table: "Cars",
                column: "CodeVIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_CodeVIN",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "CodeVIN",
                table: "Cars",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cars",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
