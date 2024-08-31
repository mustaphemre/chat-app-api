using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class ChatMessage : Entity
{
    public ChatMessage(
        Guid id,
        Guid chatId,
        Guid senderId,
        string content,
        DateTime sentUTC,
        ChatMessageStatus status
        ) : base(id)
    {
        ChatId = chatId;
        SenderId = senderId;
        Content = content;
        SentUTC = sentUTC;
        Status = status;
    }

    public Guid ChatId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime SentUTC { get; private set; }
    public ChatMessageStatus Status { get; private set; }

    public Chat Chat { get; private set; } = null!;
    public User SenderUser { get; private set; } = null!;

    public enum ChatMessageStatus
    {
        Sent,
        Delivered,
        Seen
    }
}