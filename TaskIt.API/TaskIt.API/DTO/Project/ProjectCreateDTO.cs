using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.DTO.Project;

public record ProjectCreateDTO
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public Priority? Priority { get; set; }

    public DateTime? GoalDate { get; set; }
}