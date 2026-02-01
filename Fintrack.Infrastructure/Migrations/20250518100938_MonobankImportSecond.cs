using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fintrack.Infrastructure.Migrations;

/// <inheritdoc />
public partial class MonobankImportSecond : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Mcc_TransactionTypes_TransactionTypeId",
            table: "Mcc");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Mcc",
            table: "Mcc");

        migrationBuilder.DropIndex(
            name: "IX_Mcc_TransactionTypeId",
            table: "Mcc");

        migrationBuilder.DropColumn(
            name: "TransactionTypeId",
            table: "Mcc");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Mcc",
            table: "Mcc",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "MccTransactionType",
            columns: table => new
            {
                MccsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TransactionTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MccTransactionType", x => new { x.MccsId, x.TransactionTypesId });
                table.ForeignKey(
                    name: "FK_MccTransactionType_Mcc_MccsId",
                    column: x => x.MccsId,
                    principalTable: "Mcc",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MccTransactionType_TransactionTypes_TransactionTypesId",
                    column: x => x.TransactionTypesId,
                    principalTable: "TransactionTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "TransactionTypeTemplates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                IsExpense = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TransactionTypeTemplates", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "MccTransactionTypeTemplate",
            columns: table => new
            {
                MccsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TransactionTypeTemplatesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_MccTransactionTypeTemplate", x => new { x.MccsId, x.TransactionTypeTemplatesId });
                table.ForeignKey(
                    name: "FK_MccTransactionTypeTemplate_Mcc_MccsId",
                    column: x => x.MccsId,
                    principalTable: "Mcc",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_MccTransactionTypeTemplate_TransactionTypeTemplates_TransactionTypeTemplatesId",
                    column: x => x.TransactionTypeTemplatesId,
                    principalTable: "TransactionTypeTemplates",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_MccTransactionType_TransactionTypesId",
            table: "MccTransactionType",
            column: "TransactionTypesId");

        migrationBuilder.CreateIndex(
            name: "IX_MccTransactionTypeTemplate_TransactionTypeTemplatesId",
            table: "MccTransactionTypeTemplate",
            column: "TransactionTypeTemplatesId");

        var embeddedSql = @"
BEGIN TRANSACTION;

-- 1. Declare GUIDs for all TransactionTypeTemplates
DECLARE @GroceriesDiningId UNIQUEIDENTIFIER = NEWID();
DECLARE @ShoppingRetailId UNIQUEIDENTIFIER = NEWID();
DECLARE @TransportationAutoId UNIQUEIDENTIFIER = NEWID();
DECLARE @HomeUtilitiesId UNIQUEIDENTIFIER = NEWID();
DECLARE @HealthWellnessId UNIQUEIDENTIFIER = NEWID();
DECLARE @EntertainmentLeisureId UNIQUEIDENTIFIER = NEWID();
DECLARE @FinancialServicesId UNIQUEIDENTIFIER = NEWID();
DECLARE @EducationId UNIQUEIDENTIFIER = NEWID();
DECLARE @BusinessExpensesId UNIQUEIDENTIFIER = NEWID();
DECLARE @CashTransfersId UNIQUEIDENTIFIER = NEWID();
DECLARE @OtherExpenseId UNIQUEIDENTIFIER = NEWID();
DECLARE @IncomeId UNIQUEIDENTIFIER = NEWID();

-- 2. Insert TransactionTypeTemplate records (using pluralized 'TransactionTypeTemplates')
INSERT INTO TransactionTypeTemplates (Id, Name, IsExpense) VALUES
(@GroceriesDiningId, 'Groceries & Dining', 1),
(@ShoppingRetailId, 'Shopping & Retail', 1),
(@TransportationAutoId, 'Transportation & Auto', 1),
(@HomeUtilitiesId, 'Home & Utilities', 1),
(@HealthWellnessId, 'Health & Wellness', 1),
(@EntertainmentLeisureId, 'Entertainment & Leisure', 1),
(@FinancialServicesId, 'Financial & Professional Services', 1),
(@EducationId, 'Education', 1),
(@BusinessExpensesId, 'Business Expenses', 1),
(@CashTransfersId, 'Cash & Transfers (Expense)', 1),
(@OtherExpenseId, 'Other Expense', 1),
(@IncomeId, 'Income', 0);

-- 3. Insert Mcc codes and establish relationships (using pluralized 'Mcc')
-- Note: This script assumes 'MccTransactionTypeTemplate' is the join table name.
-- Note: This script assumes Mcc.Value is unique. It inserts the Mcc, then immediately looks it up by Value to link it.

-- MCC 0742
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 742);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 742;
-- MCC 0763
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 763);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 763;
-- MCC 0780
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 780);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 780;
-- MCC 1520
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1520);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1520;
-- MCC 1711
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1711);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1711;
-- MCC 1731
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1731);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1731;
-- MCC 1740
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1740);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1740;
-- MCC 1750
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1750);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1750;
-- MCC 1761
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1761);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1761;
-- MCC 1771
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1771);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 1771;
-- MCC 1799
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 1799);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 1799;
-- MCC 2741
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 2741);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 2741;
-- MCC 2791
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 2791);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 2791;
-- MCC 2842
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 2842);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 2842;
-- MCC 3000
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 3000);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 3000;
-- MCC 3351
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 3351);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 3351;
-- MCC 3501
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 3501);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 3501;
-- MCC 4011
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4011);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 4011;
-- MCC 4111
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4111);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4111;
-- MCC 4112
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4112);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4112;
-- MCC 4119
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4119);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 4119;
-- MCC 4121
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4121);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4121;
-- MCC 4131
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4131);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4131;
-- MCC 4214
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4214);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 4214;
-- MCC 4215
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4215);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 4215;
-- MCC 4225
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4225);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 4225;
-- MCC 4304
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4304);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 4304;
-- MCC 4411
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4411);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 4411;
-- MCC 4457
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4457);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 4457;
-- MCC 4468
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4468);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 4468;
-- MCC 4511
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4511);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4511;
-- MCC 4582
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4582);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4582;
-- MCC 4722
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4722);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 4722;
-- MCC 4723
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4723);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 4723;
-- MCC 4729
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4729);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4729;
-- MCC 4784
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4784);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4784;
-- MCC 4789
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4789);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 4789;
-- MCC 4812
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4812);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4812;
-- MCC 4813
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4813);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4813;
-- MCC 4814
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4814);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4814;
-- MCC 4815
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4815);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4815;
-- MCC 4816
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4816);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4816;
-- MCC 4821
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4821);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4821;
-- MCC 4829
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4829);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 4829;
-- MCC 4899
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4899);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4899;
-- MCC 4900
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 4900);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 4900;
-- MCC 5013
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5013);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5013;
-- MCC 5021
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5021);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5021;
-- MCC 5039
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5039);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5039;
-- MCC 5044
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5044);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5044;
-- MCC 5045
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5045);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5045;
-- MCC 5046
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5046);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5046;
-- MCC 5047
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5047);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 5047;
-- MCC 5051
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5051);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5051;
-- MCC 5065
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5065);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5065;
-- MCC 5072
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5072);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5072;
-- MCC 5074
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5074);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5074;
-- MCC 5085
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5085);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5085;
-- MCC 5094
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5094);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5094;
-- MCC 5099
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5099);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5099;
-- MCC 5111
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5111);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5111;
-- MCC 5122
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5122);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 5122;
-- MCC 5131
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5131);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5131;
-- MCC 5137
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5137);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5137;
-- MCC 5139
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5139);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5139;
-- MCC 5169
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5169);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5169;
-- MCC 5172
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5172);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5172;
-- MCC 5192
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5192);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5192;
-- MCC 5193
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5193);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5193;
-- MCC 5198
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5198);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5198;
-- MCC 5199
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5199);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5199;
-- MCC 5200
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5200);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5200;
-- MCC 5211
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5211);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5211;
-- MCC 5231
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5231);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5231;
-- MCC 5251
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5251);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5251;
-- MCC 5261
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5261);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5261;
-- MCC 5262
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5262);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5262;
-- MCC 5271
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5271);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5271;
-- MCC 5297
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5297);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5297;
-- MCC 5298
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5298);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5298;
-- MCC 5300
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5300);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5300;
-- MCC 5309
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5309);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5309;
-- MCC 5310
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5310);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5310;
-- MCC 5311
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5311);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5311;
-- MCC 5331
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5331);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5331;
-- MCC 5399
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5399);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5399;
-- MCC 5411
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5411);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5411;
-- MCC 5412
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5412);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5412;
-- MCC 5422
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5422);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5422;
-- MCC 5441
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5441);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5441;
-- MCC 5451
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5451);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5451;
-- MCC 5462
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5462);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5462;
-- MCC 5499
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5499);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5499;
-- MCC 5511
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5511);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5511;
-- MCC 5521
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5521);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5521;
-- MCC 5531
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5531);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5531;
-- MCC 5532
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5532);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5532;
-- MCC 5533
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5533);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5533;
-- MCC 5541
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5541);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5541;
-- MCC 5542
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5542);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5542;
-- MCC 5551
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5551);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5551;
-- MCC 5552
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5552);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5552;
-- MCC 5561
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5561);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5561;
-- MCC 5571
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5571);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 5571;
-- MCC 5592
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5592);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5592;
-- MCC 5598
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5598);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5598;
-- MCC 5599
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5599);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5599;
-- MCC 5611
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5611);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5611;
-- MCC 5621
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5621);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5621;
-- MCC 5631
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5631);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5631;
-- MCC 5641
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5641);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5641;
-- MCC 5651
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5651);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5651;
-- MCC 5655
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5655);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5655;
-- MCC 5661
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5661);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5661;
-- MCC 5681
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5681);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5681;
-- MCC 5691
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5691);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5691;
-- MCC 5697
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5697);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5697;
-- MCC 5698
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5698);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5698;
-- MCC 5699
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5699);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5699;
-- MCC 5712
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5712);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5712;
-- MCC 5713
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5713);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5713;
-- MCC 5714
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5714);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5714;
-- MCC 5715
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5715);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5715;
-- MCC 5718
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5718);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5718;
-- MCC 5719
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5719);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5719;
-- MCC 5722
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5722);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5722;
-- MCC 5732
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5732);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5732;
-- MCC 5733
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5733);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5733;
-- MCC 5734
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5734);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5734;
-- MCC 5735
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5735);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5735;
-- MCC 5811
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5811);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5811;
-- MCC 5812
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5812);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5812;
-- MCC 5813
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5813);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5813;
-- MCC 5814
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5814);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5814;
-- MCC 5815
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5815);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5815;
-- MCC 5816
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5816);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5816;
-- MCC 5817
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5817);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5817;
-- MCC 5818
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5818);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5818;
-- MCC 5912
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5912);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 5912;
-- MCC 5921
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5921);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @GroceriesDiningId FROM Mcc m WHERE m.Value = 5921;
-- MCC 5931
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5931);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5931;
-- MCC 5932
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5932);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5932;
-- MCC 5933
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5933);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 5933;
-- MCC 5935
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5935);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 5935;
-- MCC 5937
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5937);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5937;
-- MCC 5940
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5940);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5940;
-- MCC 5941
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5941);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5941;
-- MCC 5942
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5942);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5942;
-- MCC 5943
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5943);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5943;
-- MCC 5944
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5944);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5944;
-- MCC 5945
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5945);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5945;
-- MCC 5946
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5946);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5946;
-- MCC 5947
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5947);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5947;
-- MCC 5948
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5948);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5948;
-- MCC 5949
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5949);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5949;
-- MCC 5950
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5950);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5950;
-- MCC 5960
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5960);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 5960;
-- MCC 5961
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5961);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5961;
-- MCC 5962
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5962);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5962;
-- MCC 5963
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5963);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5963;
-- MCC 5964
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5964);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5964;
-- MCC 5965
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5965);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5965;
-- MCC 5966
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5966);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5966;
-- MCC 5967
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5967);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5967;
-- MCC 5968
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5968);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5968;
-- MCC 5969
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5969);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5969;
-- MCC 5970
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5970);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5970;
-- MCC 5971
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5971);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 5971;
-- MCC 5972
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5972);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5972;
-- MCC 5973
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5973);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5973;
-- MCC 5975
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5975);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 5975;
-- MCC 5976
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5976);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 5976;
-- MCC 5977
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5977);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5977;
-- MCC 5978
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5978);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 5978;
-- MCC 5983
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5983);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5983;
-- MCC 5992
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5992);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5992;
-- MCC 5993
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5993);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5993;
-- MCC 5994
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5994);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5994;
-- MCC 5995
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5995);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5995;
-- MCC 5996
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5996);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 5996;
-- MCC 5997
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5997);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5997;
-- MCC 5998
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5998);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5998;
-- MCC 5999
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 5999);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 5999;
-- MCC 6009
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6009);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6009;
-- MCC 6010
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6010);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6010;
-- MCC 6011
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6011);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6011;
-- MCC 6012
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6012);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6012;
-- MCC 6050
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6050);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6050;
-- MCC 6051
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6051);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6051;
-- MCC 6211
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6211);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6211;
-- MCC 6300
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6300);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6300;
-- MCC 6381
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6381);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6381;
-- MCC 6513
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6513);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 6513;
-- MCC 6532
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6532);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6532;
-- MCC 6533
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6533);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6533;
-- MCC 6535
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6535);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6535;
-- MCC 6536
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6536);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @IncomeId FROM Mcc m WHERE m.Value = 6536;
-- MCC 6537
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6537);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @IncomeId FROM Mcc m WHERE m.Value = 6537;
-- MCC 6538
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6538);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6538;
-- MCC 6540
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6540);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @CashTransfersId FROM Mcc m WHERE m.Value = 6540;
-- MCC 6611
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6611);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 6611;
-- MCC 6760
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 6760);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 6760;
-- MCC 7011
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7011);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7011;
-- MCC 7012
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7012);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7012;
-- MCC 7032
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7032);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7032;
-- MCC 7033
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7033);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7033;
-- MCC 7210
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7210);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7210;
-- MCC 7211
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7211);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7211;
-- MCC 7216
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7216);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7216;
-- MCC 7217
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7217);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7217;
-- MCC 7221
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7221);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7221;
-- MCC 7230
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7230);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 7230;
-- MCC 7251
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7251);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7251;
-- MCC 7261
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7261);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 7261;
-- MCC 7273
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7273);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7273;
-- MCC 7276
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7276);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 7276;
-- MCC 7277
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7277);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 7277;
-- MCC 7278
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7278);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7278;
-- MCC 7296
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7296);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7296;
-- MCC 7297
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7297);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 7297;
-- MCC 7298
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7298);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 7298;
-- MCC 7299
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7299);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 7299;
-- MCC 7311
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7311);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7311;
-- MCC 7321
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7321);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 7321;
-- MCC 7333
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7333);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7333;
-- MCC 7338
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7338);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7338;
-- MCC 7339
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7339);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7339;
-- MCC 7342
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7342);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7342;
-- MCC 7349
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7349);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7349;
-- MCC 7361
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7361);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 7361;
-- MCC 7372
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7372);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7372;
-- MCC 7375
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7375);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7375;
-- MCC 7379
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7379);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7379;
-- MCC 7389
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7389);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7389;
-- MCC 7392
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7392);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 7392;
-- MCC 7393
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7393);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7393;
-- MCC 7394
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7394);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7394;
-- MCC 7395
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7395);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7395;
-- MCC 7399
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7399);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 7399;
-- MCC 7512
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7512);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7512;
-- MCC 7513
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7513);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7513;
-- MCC 7519
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7519);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7519;
-- MCC 7523
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7523);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7523;
-- MCC 7531
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7531);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7531;
-- MCC 7534
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7534);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7534;
-- MCC 7535
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7535);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7535;
-- MCC 7538
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7538);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7538;
-- MCC 7542
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7542);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7542;
-- MCC 7549
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7549);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 7549;
-- MCC 7622
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7622);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7622;
-- MCC 7623
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7623);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7623;
-- MCC 7629
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7629);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7629;
-- MCC 7631
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7631);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 7631;
-- MCC 7641
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7641);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7641;
-- MCC 7692
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7692);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 7692;
-- MCC 7699
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7699);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 7699;
-- MCC 7800
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7800);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7800;
-- MCC 7801
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7801);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7801;
-- MCC 7802
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7802);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7802;
-- MCC 7829
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7829);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7829;
-- MCC 7832
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7832);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7832;
-- MCC 7841
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7841);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7841;
-- MCC 7911
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7911);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7911;
-- MCC 7922
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7922);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7922;
-- MCC 7929
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7929);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7929;
-- MCC 7932
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7932);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7932;
-- MCC 7933
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7933);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7933;
-- MCC 7941
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7941);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7941;
-- MCC 7991
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7991);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7991;
-- MCC 7992
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7992);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7992;
-- MCC 7993
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7993);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7993;
-- MCC 7994
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7994);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7994;
-- MCC 7995
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7995);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7995;
-- MCC 7996
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7996);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7996;
-- MCC 7997
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7997);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7997;
-- MCC 7998
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7998);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7998;
-- MCC 7999
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 7999);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 7999;
-- MCC 8011
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8011);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8011;
-- MCC 8021
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8021);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8021;
-- MCC 8031
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8031);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8031;
-- MCC 8041
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8041);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8041;
-- MCC 8042
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8042);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8042;
-- MCC 8043
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8043);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8043;
-- MCC 8049
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8049);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8049;
-- MCC 8050
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8050);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8050;
-- MCC 8062
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8062);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8062;
-- MCC 8071
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8071);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8071;
-- MCC 8099
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8099);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HealthWellnessId FROM Mcc m WHERE m.Value = 8099;
-- MCC 8111
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8111);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 8111;
-- MCC 8211
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8211);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EducationId FROM Mcc m WHERE m.Value = 8211;
-- MCC 8220
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8220);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EducationId FROM Mcc m WHERE m.Value = 8220;
-- MCC 8241
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8241);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EducationId FROM Mcc m WHERE m.Value = 8241;
-- MCC 8244
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8244);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EducationId FROM Mcc m WHERE m.Value = 8244;
-- MCC 8249
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8249);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EducationId FROM Mcc m WHERE m.Value = 8249;
-- MCC 8299
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8299);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8299;
-- MCC 8351
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8351);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8351;
-- MCC 8398
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8398);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8398;
-- MCC 8641
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8641);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8641;
-- MCC 8651
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8651);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8651;
-- MCC 8661
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8661);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8661;
-- MCC 8675
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8675);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 8675;
-- MCC 8699
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8699);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 8699;
-- MCC 8734
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8734);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 8734;
-- MCC 8911
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8911);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 8911;
-- MCC 8931
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8931);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 8931;
-- MCC 8999
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 8999);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @FinancialServicesId FROM Mcc m WHERE m.Value = 8999;
-- MCC 9211
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9211);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 9211;
-- MCC 9222
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9222);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 9222;
-- MCC 9223
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9223);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 9223;
-- MCC 9311
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9311);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 9311;
-- MCC 9399
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9399);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 9399;
-- MCC 9402
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9402);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @HomeUtilitiesId FROM Mcc m WHERE m.Value = 9402;
-- MCC 9405
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9405);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @OtherExpenseId FROM Mcc m WHERE m.Value = 9405;
-- MCC 9406
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9406);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 9406;
-- MCC 9751
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9751);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @ShoppingRetailId FROM Mcc m WHERE m.Value = 9751;
-- MCC 9752
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9752);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @TransportationAutoId FROM Mcc m WHERE m.Value = 9752;
-- MCC 9754
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9754);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @EntertainmentLeisureId FROM Mcc m WHERE m.Value = 9754;
-- MCC 9950
INSERT INTO Mcc (Id, Value) VALUES (NEWID(), 9950);
INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
  SELECT m.Id, @BusinessExpensesId FROM Mcc m WHERE m.Value = 9950;


-- 4. Add MCC -1 (Other Income)
-- This checks if MCC -1 already exists before inserting
IF NOT EXISTS (SELECT 1 FROM Mcc WHERE Value = -1)
BEGIN
    DECLARE @MccOtherIncomeId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Mcc (Id, Value) VALUES (@MccOtherIncomeId, -1);
    
    INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
    VALUES (@MccOtherIncomeId, @IncomeId);
    
    PRINT 'Added MCC -1 for ""Other income"" and linked to ""Income"" template.';
END
ELSE
BEGIN
    PRINT 'MCC -1 for ""Other income"" already exists.';
END

-- 5. Add MCC -2 (Other Expense)
-- This checks if MCC -2 already exists before inserting
IF NOT EXISTS (SELECT 1 FROM Mcc WHERE Value = -2)
BEGIN
    DECLARE @MccOtherExpenseId UNIQUEIDENTIFIER = NEWID();
    INSERT INTO Mcc (Id, Value) VALUES (@MccOtherExpenseId, -2);
    
    INSERT INTO MccTransactionTypeTemplate (MccsId, TransactionTypeTemplatesId)
    VALUES (@MccOtherExpenseId, @OtherExpenseId);
    
    PRINT 'Added MCC -2 for ""Other expense"" and linked to ""Other Expense"" template.';
END
ELSE
BEGIN
    PRINT 'MCC -2 for ""Other expense"" already exists.';
END


COMMIT TRANSACTION;
";
        migrationBuilder.Sql(embeddedSql);

        migrationBuilder.CreateIndex(
            name: "IX_Mcc_Value",
            table: "Mcc",
            column: "Value",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "MccTransactionType");

        migrationBuilder.DropTable(
            name: "MccTransactionTypeTemplate");

        migrationBuilder.DropTable(
            name: "TransactionTypeTemplates");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Mcc",
            table: "Mcc");

        migrationBuilder.AddColumn<Guid>(
            name: "TransactionTypeId",
            table: "Mcc",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_Mcc",
            table: "Mcc",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_Mcc_TransactionTypeId",
            table: "Mcc",
            column: "TransactionTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Mcc_TransactionTypes_TransactionTypeId",
            table: "Mcc",
            column: "TransactionTypeId",
            principalTable: "TransactionTypes",
            principalColumn: "Id");

        migrationBuilder.Sql(@"
                DELETE FROM MccTransactionTypeTemplate WHERE MccsId IN (SELECT Id FROM Mcc WHERE Value >= -2 AND Value <= 9950);
                DELETE FROM Mcc WHERE Value >= -2 AND Value <= 9950;
                DELETE FROM TransactionTypeTemplates WHERE Name IN (
                    'Groceries & Dining', 'Shopping & Retail', 'Transportation & Auto', 'Home & Utilities',
                    'Health & Wellness', 'Entertainment & Leisure', 'Financial & Professional Services',
                    'Education', 'Business Expenses', 'Cash & Transfers (Expense)', 'Other Expense', 'Income'
                );
            ");

        migrationBuilder.DropIndex(
            name: "IX_Mcc_Value",
            table: "Mcc");
    }
}
