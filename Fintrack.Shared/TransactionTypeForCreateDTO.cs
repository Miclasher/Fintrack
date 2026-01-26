using System.ComponentModel.DataAnnotations;

namespace Fintrack.Shared;

public sealed class TransactionTypeForCreateDTO
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public bool IsExpense { get; set; }
}
