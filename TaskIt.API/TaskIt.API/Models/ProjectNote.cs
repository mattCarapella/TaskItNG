using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Models;

public class ProjectNote : Note
{
    [Required]
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}