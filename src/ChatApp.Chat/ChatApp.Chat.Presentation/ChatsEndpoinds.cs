using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Chat.Presentation;

public static class ChatsEndpoinds
{
    public static WebApplication AddChatsEndpoints(this WebApplication app)
    {
        app.MapGet("/{userId:int}", (ISender sender) =>
        {
            return Results.Ok();
        });

        app.MapGet("/{userId:int}", () =>
        {
            return Results.Ok();
        });

        return app;
    }
}