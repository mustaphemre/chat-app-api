using ChatApp.Chat.Domain.Primitives;

namespace ChatApp.Chat.Domain.Entities;

public class ChatParticipant : Entity
{
    public ChatParticipant(
        Guid id,
        Guid chatId,
        Guid userId,
        DateTime joinedUtc) : base(id)
    {
        ChatId = chatId;
        UserId = userId;
        JoinedUTC = joinedUtc;
    }

    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime JoinedUTC { get; private set; }
}
