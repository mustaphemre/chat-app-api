using ChatApp.Chats.Application.Models;
using ChatApp.Chats.Infrastructure;
using MediatR;
using static UsersService.Grpc.UsersService;
using ChatEntity = ChatApp.Chats.Domain.Chats;

namespace ChatApp.Chats.Application.Chats;

public class CreateChatHandler : IRequestHandler<CreateChatInput, CreateChatOutput>
{
    private readonly ChatDbContext _dbContext;
    private readonly UsersServiceClient _usersServiceClient;

    public CreateChatHandler(
        ChatDbContext dbContext,
        UsersServiceClient usersServiceClient)
    {
        _dbContext = dbContext;
        _usersServiceClient = usersServiceClient;
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

        chat.AddParticipant(creator.UserId);

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

    private async Task<UserOutput> GetUserAsync(Guid userId)
    {
        var userDto = await _usersServiceClient.GetUserByIdAsync(new UsersService.Grpc.GetUserByIdRequest { UserId = userId.ToString() });

        return new UserOutput
        {
            UserId = Guid.Parse(userDto.UserId),
            Username = userDto.Username,
            Email = userDto.Email,
            ProfilePicture = userDto.ProfilePicture,
            Status = userDto.Status,
        };
    }
}