namespace TaskIt.API.DTO;

public record LinkDTO
{
    public LinkDTO(string href, string rel, string type)
    {
        Href = href;
        Rel = rel;
        Type = type;
    }

    public string Href { get; set; } = string.Empty;
    public string Rel { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
