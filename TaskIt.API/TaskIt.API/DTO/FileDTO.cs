using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.DTO;

public record FileDTO
{
    [Required]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public byte[]? Data { get; set; }

    public string? FileType { get; set; }

    public string? Extension { get; set; }

    public long? FileSize { get; set; }

}