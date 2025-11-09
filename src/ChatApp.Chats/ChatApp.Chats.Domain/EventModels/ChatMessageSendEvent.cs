namespace ChatApp.Chats.Domain.EventModels;

public class ChatMessageSendEvent
{
    public string ChatId { get; set; } = null!;
    public string SenderId { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime SentUTC { get; set; }
}
