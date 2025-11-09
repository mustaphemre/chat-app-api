using ChatApp.Chats.Application.Chats;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ChatApp.Chats.Presentation.Endpoints;

public static class ChatsEndpoinds
{
    public static WebApplication AddChatsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("chat");

        group.MapPost("/create", (CreateChatInput input, ISender sender) =>
        {
            return sender.Send(input);
        });

        return app;
    }
}