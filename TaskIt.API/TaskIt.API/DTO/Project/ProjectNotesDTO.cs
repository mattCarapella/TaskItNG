using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.DTO.Project;

public record ProjectNotesDTO
{
    [Required]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public List<string>? Links { get; set; }

    public bool? Flagged { get; set; }
}