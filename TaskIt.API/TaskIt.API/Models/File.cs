using System.ComponentModel.DataAnnotations;

namespace TaskIt.API.Models;

public class File
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public byte[] Data { get; set; } = Array.Empty<byte>();

    [Required]
    [MaxLength(50)]
    public string FileType { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Extension { get; set; } = string.Empty;

    public long FileSize { get; set; }

    //public string UploadedByUserId { get; set; } = String.Empty;

    //public ApplicationUser UploadedByUser { get; set; }

    [Required]
    public DateTime DateCreated { get; set; }

    [Required]
    public DateTime LastModified { get; set; }
}
