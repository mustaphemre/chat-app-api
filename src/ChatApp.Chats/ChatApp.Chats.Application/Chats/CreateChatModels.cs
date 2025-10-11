using MediatR;

namespace ChatApp.Chats.Application.Chats
{
    public record CreateChatInput : IRequest<CreateChatOutput>
    {
        public bool IsGroupChat { get; set; }
        public Guid CreatorId { get; set; }
        public List<ChatParticipationInput> ChatParticipations { get; set; } = null!;
    }

    public record ChatParticipationInput
    {
        public Guid UserId { get; private set; }
    }

    public record CreateChatOutput
    {
        public bool Success { get; set; }
        public Guid? ChatId { get; set; }
    }
}
