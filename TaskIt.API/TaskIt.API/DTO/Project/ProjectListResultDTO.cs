using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.DTO.Project;

public record ProjectListResultDTO
{
    [Required]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Archived { get; set; }

    public bool? Flagged { get; set; }

    public Status? Status { get; set; }

    public Priority? Priority { get; set; }

    public DateTime? GoalDate { get; set; }

    public DateTime? DateClosed { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? TicketCount { get; set; }
}