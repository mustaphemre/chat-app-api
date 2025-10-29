using ChatApp.Chats.Application.IntegrationServices;
using ChatApp.Chats.Application.IntegrationServices.Dtos;
using ChatApp.Chats.Infrastructure;
using MediatR;
using ChatEntity = ChatApp.Chats.Domain.Chats;

namespace ChatApp.Chats.Application.Chats;

public class CreateChatHandler : IRequestHandler<CreateChatInput, CreateChatOutput>
{
    private readonly ChatDbContext _dbContext;
    private readonly IUsersIntegrationService _usersIntegrationService;

    public CreateChatHandler(
        ChatDbContext dbContext,
        IUsersIntegrationService usersIntegrationService)
    {
        _dbContext = dbContext;
        _usersIntegrationService = usersIntegrationService;
    }

    public async Task<CreateChatOutput> Handle(CreateChatInput request, CancellationToken cancellationToken)
    {
        var creator = await _usersIntegrationService.GetUserAsync(new GetUserInput(request.CreatorId), cancellationToken);
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

        chat.AddParticipant(creator.UserId);

        foreach (var participation in request.ChatParticipations)
        {
            var participant = await _usersIntegrationService.GetUserAsync(new GetUserInput(participation.UserId), cancellationToken);
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
}