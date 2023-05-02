using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.Models;

public class Ticket
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength (5000)]
    public string Description { get; set; } = string.Empty;

    public bool Flagged { get; set; } = false;

    public bool Archived { get; set; } = false;

    public int Votes { get; set; } = 0;


    public TicketType Type { get; set; }

    public Status Status { get; set; }

    public Priority Priority { get; set; }


    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public DateTime LastModified { get; set; }

    public DateTime? GoalDate { get; set; }

    public DateTime? DateClosed { get; set; }


    [Required]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }


    public ICollection<TicketNote>? Notes { get; set; } = new List<TicketNote>();

    public ICollection<TicketFile>? Files { get; set; } = new List<TicketFile>();
}