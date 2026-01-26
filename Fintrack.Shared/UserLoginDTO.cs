using System.ComponentModel.DataAnnotations;

namespace Fintrack.Shared;

public sealed class UserLoginDTO
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
