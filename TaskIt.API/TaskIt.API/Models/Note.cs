using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Models;

public class Note
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    public List<string>? Links = new List<string>();

    public bool Flagged { get; set; } = false;


    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public DateTime LastModified { get; set; }

}