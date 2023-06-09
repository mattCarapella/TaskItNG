﻿using System.ComponentModel.DataAnnotations;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.DTO.Project;

public record ProjectUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public bool? Archived { get; set; }

    public bool? Flagged { get; set; }

    public Status? Status { get; set; }

    public Priority? Priority { get; set; }

    public DateTime? GoalDate { get; set; }

    public DateTime? DateClosed { get; set; }
}
