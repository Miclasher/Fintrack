using ClosedXML.Excel;
using Fintrack.Services.Abstractions;
using Fintrack.Shared;

namespace Fintrack.Services;

public sealed class SummaryExcelExportService : ISummaryExcelExportService
{
    public byte[] GenerateExcel(SummaryDTO summary, IEnumerable<TransactionTypeDTO> transactionTypes)
    {
        ArgumentNullException.ThrowIfNull(summary);
        ArgumentNullException.ThrowIfNull(transactionTypes);

        var transactionTypeById = transactionTypes.ToDictionary(e => e.Id);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Summary");

        worksheet.Cell(1, 2).Value = "Income";
        worksheet.Cell(1, 1).Value = summary.TotalIncome;
        worksheet.Cell(2, 2).Value = "Expenses";
        worksheet.Cell(2, 1).Value = -summary.TotalExpense;

        worksheet.Cell(4, 1).Value = "Amount";
        worksheet.Cell(4, 2).Value = "Date";
        worksheet.Cell(4, 3).Value = "Comment";
        worksheet.Cell(4, 4).Value = "Transaction Type";

        var row = 5;

        foreach (var operation in summary.Operations.OrderByDescending(x => x.Date))
        {
            var transactionType = transactionTypeById.GetValueOrDefault(operation.TransactionTypeId);
            var signedAmount = transactionType?.IsExpense == true ? -operation.Amount : operation.Amount;

            worksheet.Cell(row, 1).Value = signedAmount;
            worksheet.Cell(row, 2).Value = operation.Date;
            worksheet.Cell(row, 3).Value = operation.UserComment ?? string.Empty;
            worksheet.Cell(row, 4).Value = transactionType?.Name ?? "Unknown";

            row++;
        }

        var headerRange = worksheet.Range(4, 1, 4, 4);
        headerRange.Style.Font.Bold = true;

        worksheet.Column(1).Style.NumberFormat.Format = "#,##0.00";
        worksheet.Column(2).Style.DateFormat.Format = "yyyy-mm-dd hh:mm";
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
