namespace ChatApp.Chats.Application.Models;

public record UserOutput
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Status { get; set; }
    public string? ProfilePicture { get; set; }
}
