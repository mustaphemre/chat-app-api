using ChatEntity = ChatApp.Chats.Domain.Chats;
using ChatApp.Chats.Infrastructure;
using MediatR;
using ChatApp.Chats.Application.Models;

namespace ChatApp.Chats.Application.Chats;

public class CreateChatHandler : IRequestHandler<CreateChatInput, CreateChatOutput>
{
    private readonly ChatDbContext _dbContext;

    public CreateChatHandler(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateChatOutput> Handle(CreateChatInput request, CancellationToken cancellationToken)
    {
        var creator = await GetUserAsync(request.CreatorId);
        if (creator is null)
        {
            throw new Exception("Creator user not found!");
        }

        var chatId = Guid.NewGuid();

        var chat = new ChatEntity.Chat(
            chatId,
            request.IsGroupChat,
            creator.UserId,
            DateTime.UtcNow,
            null);

        foreach (var participation in request.ChatParticipations)
        {
            var participant = await GetUserAsync(participation.UserId);
            if (participant is null)
            {
                throw new Exception("Participant user not found!");
            }

            chat.AddParticipant(participation.UserId);
        }

        await _dbContext.Chats.AddAsync(chat, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateChatOutput
        {
            ChatId = chatId,
            Success = true,
        };
    }

    private Task<UserOutput> GetUserAsync(Guid userId) => Task.FromResult(new UserOutput
    {
        UserId = Guid.NewGuid(),
        Username = string.Empty,
        Email = string.Empty,
        ProfilePicture = string.Empty,
        Status = string.Empty
    });
}