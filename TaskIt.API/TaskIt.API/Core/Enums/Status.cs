using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Core.Enums;

public enum Status
{
    [Display(Name = "Unassigned")]
    UNASSIGNED,
    [Display(Name = "In Progress")]
    INPROGRESS,
    [Display(Name = "Submitted")]
    SUBMITTED,
    [Display(Name = "Completed")]
    COMPLETED
}