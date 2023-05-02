using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Core.Enums;

public enum Priority
{
    [Display(Name = "Low")]
    LOW,

    [Display(Name = "Medium")]
    MED,
    
    [Display(Name = "High")]
    HIGH,

    [Display(Name = "Critical")]
    CRITICAL
}