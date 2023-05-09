using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.DTO.Account;

public record RegisterRequestDTO
{
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; }

}