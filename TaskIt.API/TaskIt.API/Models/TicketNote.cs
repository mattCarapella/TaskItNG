using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Models;

public class TicketNote : Note
{
    [Required]
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
}