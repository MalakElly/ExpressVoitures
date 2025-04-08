using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionVoituresExpress.Migrations
{
    /// <inheritdoc />
    public partial class TableJointureRepairing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairing_Transactions_TransactionId",
                table: "Repairing");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Repairing");

            migrationBuilder.DropColumn(
                name: "RepairingType",
                table: "Repairing");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Transactions",
                newName: "SellingPrice");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Repairing",
                newName: "CarID");

            migrationBuilder.RenameColumn(
                name: "RepairDate",
                table: "Repairing",
                newName: "RepairingDate");

            migrationBuilder.RenameIndex(
                name: "IX_Repairing_TransactionId",
                table: "Repairing",
                newName: "IX_Repairing_CarID");

            migrationBuilder.AddColumn<double>(
                name: "BuyingPrice",
                table: "Transactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "RepairingType",
                columns: table => new
                {
                    RepairingTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairingName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairingType", x => x.RepairingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "RepairingAndType",
                columns: table => new
                {
                    RepairingId = table.Column<int>(type: "int", nullable: false),
                    RepairingTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairingAndType", x => new { x.RepairingId, x.RepairingTypeId });
                    table.ForeignKey(
                        name: "FK_RepairingAndType_RepairingType_RepairingTypeId",
                        column: x => x.RepairingTypeId,
                        principalTable: "RepairingType",
                        principalColumn: "RepairingTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairingAndType_Repairing_RepairingId",
                        column: x => x.RepairingId,
                        principalTable: "Repairing",
                        principalColumn: "RepairingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairingAndType_RepairingTypeId",
                table: "RepairingAndType",
                column: "RepairingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairing_Cars_CarID",
                table: "Repairing",
                column: "CarID",
                principalTable: "Cars",
                principalColumn: "CarID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairing_Cars_CarID",
                table: "Repairing");

            migrationBuilder.DropTable(
                name: "RepairingAndType");

            migrationBuilder.DropTable(
                name: "RepairingType");

            migrationBuilder.DropColumn(
                name: "BuyingPrice",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Transactions",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "RepairingDate",
                table: "Repairing",
                newName: "RepairDate");

            migrationBuilder.RenameColumn(
                name: "CarID",
                table: "Repairing",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Repairing_CarID",
                table: "Repairing",
                newName: "IX_Repairing_TransactionId");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Repairing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RepairingType",
                table: "Repairing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Repairing_Transactions_TransactionId",
                table: "Repairing",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
