using Microsoft.EntityFrameworkCore;
using TaskIt.API.DTO;

namespace TaskIt.API.Models;

public class ApiResult<T>
{
    private ApiResult(List<T> data, int recordCount, int pageIndex, int pageSize, List<LinkDTO> links, string? sortColumn, string? sortOrder)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        RecordCount = recordCount;
        TotalPages = (int)Math.Ceiling(recordCount / (double)pageSize);
        Links = links;
        SortColumn = sortColumn;
        SortOrder = sortOrder;
    }

    //public static async Task<ApiResult<T>> CreateAsync(IQueryable<T> data, int? pageIndex, int? pageSize, List<LinkDTO> links, string? message, string? sortColumn, string? sortOrder)
    //{
    //    var count = await data.CountAsync();

    //    if (!string.IsNullOrEmpty(sortColumn) && IsValidProperty(sortColumn))
    //    {

    //    }
    //}

    //public static bool IsValidProperty(string propertyName, bool throwExceptionIfNotFound = true)
    //{

    //}

    public List<T> Data { get; set; } = default!;
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public int? TotalPages { get; set; }
    public int? RecordCount { get; set; }
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }
}