namespace Fintrack.Infrastructure.ExternalClients.Monobank;

internal sealed class MonobankTransactionResponseDTO
{
    public string Id { get; set; } = string.Empty;
    public int Time { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Mcc { get; set; }
    public int OriginalMcc { get; set; }
    public bool Hold { get; set; }
    public int Amount { get; set; }
    public int OperationAmount { get; set; }
    public int CurrencyCode { get; set; }
    public int CommissionRate { get; set; }
    public int CashbackAmount { get; set; }
    public int Balance { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string ReceiptId { get; set; } = string.Empty;
    public string InvoiceId { get; set; } = string.Empty;
    public string CounterEdrpou { get; set; } = string.Empty;
    public string CounterIban { get; set; } = string.Empty;
    public string CounterName { get; set; } = string.Empty;
}
