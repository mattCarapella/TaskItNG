using System.ComponentModel;
using TaskIt.API.Core.Enums;

namespace TaskIt.API.DTO;

public record RequestDTO<T> 
{
    [DefaultValue(0)]
    public int PageIndex { get; set; } = 0;

    [DefaultValue(10)]
    public int PageSize { get; set; } = 10;

    [DefaultValue("Id")]
    public string? SortColumn { get; set; } = "Id";

    [DefaultValue("ASC")]
    public string? SortOrder { get; set; } = "ASC";

    [DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;

    [DefaultValue(null)]
    public Status? Status { get; set; } = null;

    [DefaultValue(null)]
    public Priority? Priority { get; set; } = null;

}