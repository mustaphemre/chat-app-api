using MediatR;

namespace ChatApp.Chats.Application.Messages;

public record SendMessageInput : IRequest<SendMessageOutput>
{
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = null!;
}

public record SendMessageOutput
{
    public bool Success { get; set; }
    public Guid? MessageId { get; set; }
}
