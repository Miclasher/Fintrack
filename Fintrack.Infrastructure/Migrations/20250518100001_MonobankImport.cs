using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrack.Infrastructure.Migrations;

/// <inheritdoc />
public partial class MonobankImport : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "MonobankAccountId",
            table: "Users",
            type: "nvarchar(max)",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateTable(
            name: "Mcc",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Value = table.Column<int>(type: "int", nullable: false),
                TransactionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Mcc", x => x.Id);
                table.ForeignKey(
                    name: "FK_Mcc_TransactionTypes_TransactionTypeId",
                    column: x => x.TransactionTypeId,
                    principalTable: "TransactionTypes",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Mcc_TransactionTypeId",
            table: "Mcc",
            column: "TransactionTypeId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Mcc");

        migrationBuilder.DropColumn(
            name: "MonobankAccountId",
            table: "Users");
    }
}
