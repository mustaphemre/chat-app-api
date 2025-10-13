using ChatApp.Chats.Application.Models;
using ChatApp.Chats.Domain.Chats.Entities;
using ChatApp.Chats.Infrastructure;
using MediatR;
using static UsersService.Grpc.UsersService;

namespace ChatApp.Chats.Application.Messages;

public class SendMessageHandler : IRequestHandler<SendMessageInput, SendMessageOutput>
{
    private readonly ChatDbContext _dbContext;
    private readonly UsersServiceClient _usersServiceClient;

    public SendMessageHandler(
        ChatDbContext dbContext,
        UsersServiceClient usersServiceClient
        )
    {
        _dbContext = dbContext;
        _usersServiceClient = usersServiceClient;
    }

    public async Task<SendMessageOutput> Handle(SendMessageInput request, CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats.FindAsync(request.ChatId, cancellationToken);
        if (chat is null)
        {
            throw new Exception("Chat not found!");
        }

        var sender = await GetUserAsync(request.SenderId);
        if (sender is null)
        {
            throw new Exception("Sender not found!");
        }

        var message = new ChatMessage(
            Guid.NewGuid(),
            chat.Id,
            sender.UserId,
            request.Content,
            DateTime.UtcNow,
            ChatMessage.ChatMessageStatus.Sent);

        await _dbContext.ChatMessages.AddAsync(message, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SendMessageOutput
        {
            MessageId = message.Id,
            Success = true
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