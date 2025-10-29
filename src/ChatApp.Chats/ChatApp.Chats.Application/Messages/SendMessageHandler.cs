using ChatApp.Chats.Application.IntegrationServices;
using ChatApp.Chats.Application.IntegrationServices.Dtos;
using ChatApp.Chats.Application.Producers;
using ChatApp.Chats.Domain.Chats.Entities;
using ChatApp.Chats.Domain.EventModels;
using ChatApp.Chats.Infrastructure;
using MediatR;
using static UsersService.Grpc.UsersService;

namespace ChatApp.Chats.Application.Messages;

public class SendMessageHandler : IRequestHandler<SendMessageInput, SendMessageOutput>
{
    private readonly ChatDbContext _dbContext;
    private readonly IUsersIntegrationService _usersIntegrationService;
    private readonly ChatMessageProducer _chatMessageProducer;

    public SendMessageHandler(
        ChatDbContext dbContext,
        UsersServiceClient usersServiceClient,
        IUsersIntegrationService usersIntegrationService,
        ChatMessageProducer chatMessageProducer
        )
    {
        _dbContext = dbContext;
        _usersIntegrationService = usersIntegrationService;
        _chatMessageProducer = chatMessageProducer;
    }

    public async Task<SendMessageOutput> Handle(SendMessageInput request, CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats.FindAsync(request.ChatId, cancellationToken);
        if (chat is null)
        {
            throw new Exception("Chat not found!");
        }

        var sender = await _usersIntegrationService.GetUserAsync(new GetUserInput(request.SenderId), cancellationToken);
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

        await PushEvent(message);

        return new SendMessageOutput
        {
            MessageId = message.Id,
            Success = true
        };
    }

    private async Task PushEvent(ChatMessage chatMessage)
    {
        var message = new ChatMessageSendEvent
        {
            ChatId = chatMessage.ChatId.ToString(),
            SenderId = chatMessage.SenderId.ToString(),
            Content = chatMessage.Content,
            SentUTC = chatMessage.SentUTC
        };

        await _chatMessageProducer.PublishAsync(chatMessage.Id.ToString(), message);
    }
}