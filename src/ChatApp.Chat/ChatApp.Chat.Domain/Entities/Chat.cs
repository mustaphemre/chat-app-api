using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class Chat : Entity
{
    private readonly List<ChatParticipant> _chatParticipants = new();
    private readonly List<ChatMessage> _chatMessages = new();

    public Chat(
        Guid id,
        bool isGroupChat,
        DateTime createdUTC,
        DateTime? lastUpdatedUTC) : base(id)
    {
        IsGroupChat = false;
        CreatedUTC = DateTime.UtcNow;
        LastUpdatedUTC = null;
    }

    public bool IsGroupChat { get; private set; }
    public DateTime CreatedUTC { get; private set; }
    public DateTime? LastUpdatedUTC { get; private set; }

    public IReadOnlyCollection<ChatParticipant> ChatParticipants => _chatParticipants;
    public IReadOnlyCollection<ChatMessage> ChatMessages => _chatMessages;
}