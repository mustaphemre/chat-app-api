namespace ChatApp.Chats.Domain.EventModels;

public class ChatMessageSendEvent
{
    public string ChatId { get; set; }
    public string SenderId { get; set; }
    public string Content { get; set; }
    public DateTime SentUTC { get; set; }
}
