using Microsoft.AspNetCore.Identity;

namespace TaskIt.API.Models;

public class ApiUser : IdentityUser
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public byte[]? UserImage { get; set; }
    public string? EmployeeID { get; set; }
    public string? JobTitle { get; set; }
    public DateTime? LastLoggedIn { get; set; }

}