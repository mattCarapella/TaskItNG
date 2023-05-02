using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Core.Enums;

public enum TicketType
{
    [Display(Name = "Bug")]
    BUG,
    [Display(Name = "Feature")]
    FEATURE,
}
