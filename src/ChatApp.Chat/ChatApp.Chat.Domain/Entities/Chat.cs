using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class Chat : Entity
{
    public Chat(
        Guid id, 
        bool isGroupChat, 
        DateTime createdUtc,
        DateTime? lastUpdatedUTC) : base(id)
    {
        IsGroupChat = false;
        CreatedUTC = DateTime.UtcNow;
        LastUpdatedUTC = null;
    }

    public bool IsGroupChat { get; private set; }
    public DateTime CreatedUTC { get; private set; }
    public DateTime? LastUpdatedUTC { get; private set; }
}