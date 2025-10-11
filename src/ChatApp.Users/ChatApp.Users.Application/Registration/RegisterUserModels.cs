using MediatR;

namespace ChatApp.Users.Application.Registration;

public record RegisterUserInput : IRequest<RegisterUserOutput>
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ProfilePicture { get; set; }
}

public record RegisterUserOutput
{
    public Guid UserId { get; set; }
    public bool Success { get; set; }
}