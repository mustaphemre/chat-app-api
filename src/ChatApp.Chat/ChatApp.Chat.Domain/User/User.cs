using ChatApp.Chat.Domain.Chat.Entities;
using ChatApp.Chat.Domain.Common.Primitives;

namespace ChatApp.Chat.Domain.User;

public class User : AggregateRoot
{
    private readonly List<ChatParticipant> _participations = new();
    private readonly List<ChatMessage> _chatMessages = new();

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

    public IReadOnlyCollection<ChatParticipant> ChatParticipations => _participations;
    public IReadOnlyCollection<ChatMessage> ChatMessages => _chatMessages;

    public enum UserChatStatus
    {
        Offline,
        Online,
        Busy
    }
}
