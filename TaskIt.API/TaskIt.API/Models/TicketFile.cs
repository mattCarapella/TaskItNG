using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Models;

public class TicketFile : File
{
    [Required]
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;
}