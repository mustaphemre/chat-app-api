using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class ChatMessage : Entity
{
    public ChatMessage(
        Guid id,
        Guid chatId,
        Guid senderId,
        string contetnt,
        DateTime sentUtc,
        string status
        ) : base(id)
    {
        ChatId = chatId;
        SenderId = senderId;
        Content = contetnt;
        SentUTC = sentUtc;
        Status = status;
    }

    public Guid ChatId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentUTC { get; private set; }
    public string Status { get; private set; }
}
