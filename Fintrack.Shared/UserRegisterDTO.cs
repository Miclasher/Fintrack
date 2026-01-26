using System.ComponentModel.DataAnnotations;

namespace Fintrack.Shared;

public sealed class UserRegisterDTO
{
    [Required]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PlainPassword { get; set; } = string.Empty;
    [Required]
    [MaxLength(150)]
    public string DisplayName { get; set; } = string.Empty;
    [Required]
    public string MonobankApiToken { get; set; } = string.Empty;
}
