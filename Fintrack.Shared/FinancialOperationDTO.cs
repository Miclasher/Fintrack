using System.ComponentModel.DataAnnotations;

namespace Fintrack.Shared;

public sealed class FinancialOperationDTO
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [MaxLength(1000)]
    public string? UserComment { get; set; }
    [Required]
    public Guid TransactionTypeId { get; set; }
}
