using ChatApp.Users.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ChatApp.Chats.Presentation;

public static class UsersEndpoinds
{
    public static WebApplication AddUsersEndpoints(this WebApplication app)
    {
        app.MapGroup("users");

        app.MapGet("/{userId:guid}", (Guid userId, ISender sender) =>
        {
            return sender.Send(new GetUserByIdInput(userId));
        });

        return app;
    }
}