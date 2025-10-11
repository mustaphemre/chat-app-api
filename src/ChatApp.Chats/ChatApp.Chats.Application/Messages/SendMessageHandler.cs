using ChatApp.Chats.Application.Models;
using ChatApp.Chats.Domain.Chats.Entities;
using ChatApp.Chats.Infrastructure;
using MediatR;

namespace ChatApp.Chats.Application.Messages;

public class SendMessageHandler : IRequestHandler<SendMessageInput, SendMessageOutput>
{
    private readonly ChatDbContext _dbContext;

    public SendMessageHandler(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
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

    private Task<UserOutput> GetUserAsync(Guid userId) => Task.FromResult(new UserOutput
    {
        UserId = Guid.NewGuid(),
        Username = string.Empty,
        Email = string.Empty,
        ProfilePicture = string.Empty,
        Status = string.Empty
    });
}