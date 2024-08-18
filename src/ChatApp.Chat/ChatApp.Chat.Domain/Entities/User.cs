using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class User : Entity
{
    public User(
        Guid id,
        string username,
        string email,
        string status,
        string profilePicture) : base(id)
    {
        Username = username;
        Email = email;
        Status = status;
        ProfilePicture = profilePicture;
    }

    public string Username { get; private set; }
    public string Email { get; private set; }
    public string Status { get; private set; }
    public string ProfilePicture { get; private set; }
}
