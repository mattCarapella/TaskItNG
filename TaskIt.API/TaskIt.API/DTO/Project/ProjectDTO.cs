using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;
using TaskIt.API.DTO.Ticket;
using TaskIt.API.Models;

namespace TaskIt.API.DTO.Project;

public record ProjectDTO
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

    public IEnumerable<ProjectTicketsDTO>? Tickets { get; set; }

    public IEnumerable<ProjectNotesDTO>? Notes { get; set; }

    public int? TicketCount { get; set; } = 0;

}