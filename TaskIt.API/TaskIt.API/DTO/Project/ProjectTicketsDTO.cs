using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.DTO.Project;

public record ProjectTicketsDTO
{
    [Required]
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Flagged { get; set; }

    public string? Archived { get; set; }

    public int? Votes { get; set; }

    public TicketType? TicketType { get; set; }

    public Status? Status { get; set; }

    public Priority? Priority { get; set; }

    public DateTime? GoalDate { get; set; }

    public DateTime? DateCreated { get; set; }
}