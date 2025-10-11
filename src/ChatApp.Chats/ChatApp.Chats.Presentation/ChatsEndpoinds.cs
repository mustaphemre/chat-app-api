using ChatApp.Chats.Application.Chats;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ChatApp.Chats.Presentation;

public static class ChatsEndpoinds
{
    public static WebApplication AddChatsEndpoints(this WebApplication app)
    {
        app.MapGroup("chat");

        app.MapPost("/create", (CreateChatInput input, ISender sender) =>
        {
            return sender.Send(input);
        });

        return app;
    }
}