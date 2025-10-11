using ChatApp.Chats.Domain.Chats.Entities;
using ChatApp.Common.Primitives;

namespace ChatApp.Chats.Domain.Chats;

public class Chat : AggregateRoot
{
    private readonly List<ChatParticipant> _chatParticipants = new();
    private readonly List<ChatMessage> _chatMessages = new();

    public Chat(
        Guid id,
        bool isGroupChat,
        Guid creatorId,
        DateTime createdUTC,
        DateTime? lastUpdatedUTC) : base(id)
    {
        IsGroupChat = false;
        CreatorId = creatorId;
        CreatedUTC = DateTime.UtcNow;
        LastUpdatedUTC = null;
    }

    public bool IsGroupChat { get; private set; }
    public Guid CreatorId { get; set; }
    public DateTime CreatedUTC { get; private set; }
    public DateTime? LastUpdatedUTC { get; private set; }

    public IReadOnlyCollection<ChatParticipant> ChatParticipants => _chatParticipants;
    public IReadOnlyCollection<ChatMessage> ChatMessages => _chatMessages;

    public void AddParticipant(Guid userId)
    {
        var item = new ChatParticipant(Guid.NewGuid(), Id, userId, DateTime.UtcNow);
        _chatParticipants.Add(item);
    }
}