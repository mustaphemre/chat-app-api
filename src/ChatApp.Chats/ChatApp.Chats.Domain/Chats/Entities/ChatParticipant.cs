using ChatApp.Common.Primitives;

namespace ChatApp.Chats.Domain.Chats.Entities;

//TODO: name change ChatParticipation
public class ChatParticipant : Entity
{
    public ChatParticipant(
        Guid id,
        Guid chatId,
        Guid userId,
        DateTime joinedUTC) : base(id)
    {
        ChatId = chatId;
        UserId = userId;
        JoinedUTC = joinedUTC;
    }

    public Guid ChatId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime JoinedUTC { get; private set; }
}