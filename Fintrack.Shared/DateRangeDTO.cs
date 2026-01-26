using System.ComponentModel.DataAnnotations;

namespace Fintrack.Shared;

public sealed class DateRangeDTO
{
    [Required]
    public DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
}
