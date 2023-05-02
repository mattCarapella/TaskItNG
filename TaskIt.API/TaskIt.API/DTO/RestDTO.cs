namespace TaskIt.API.DTO;

public record RestDTO<T>
{
    public T Data { get; set; } = default!;
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
    public int? TotalPages { get; set; }
    public int? RecordCount { get; set; }
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    public string? Message { get; set; }

}