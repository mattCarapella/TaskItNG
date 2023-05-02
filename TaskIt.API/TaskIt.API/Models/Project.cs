using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.Models;

public class Project
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength (5000)]
    public string Description { get; set; } = string.Empty;

    public bool Archived { get; set; } = false;

    public bool Flagged { get; set; } = false;


    public Status Status { get; set; }

    public Priority Priority { get; set; }


    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public DateTime LastModified { get; set; }

    public DateTime? GoalDate { get; set; }

    public DateTime? DateClosed { get; set; }


    public ICollection<Ticket>? Tickets { get; set; } = new List<Ticket> ();
    
    public ICollection<ProjectNote>? Notes { get; set; } = new List<ProjectNote> ();

    public ICollection<ProjectFile>? Files { get; set; } = new List<ProjectFile> ();

}