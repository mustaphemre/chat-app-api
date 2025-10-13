using ChatApp.Chats.Application.Messages;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ChatApp.Chats.Presentation.Endpoints;

public static class MessagesEndpoinds
{
    public static WebApplication AddMessagesEndpoints(this WebApplication app)
    {
        app.MapGroup("message");

        app.MapPost("/send", (SendMessageInput input, ISender sender) =>
        {
            return sender.Send(input);
        });

        return app;
    }
}