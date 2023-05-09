namespace TaskIt.API.DTO.Account;

public record RegisterResponseDTO
{
    public bool IsSuccessfulRegistration { get; set; }
    public IEnumerable<string>? Errors { get; set; } 

}