using ChatApp.Users.Application.Registration;
using ChatApp.Users.Application.UsersQuery;
using MediatR;
using Microsoft.AspNetCore.Builder;

namespace ChatApp.Users.Presentation.Endpoints;

public static class UsersEndpoinds
{
    public static WebApplication AddUsersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");

        group.MapGet("/{userId:guid}", (Guid userId, ISender sender) =>
        {
            return sender.Send(new GetUserByIdInput(userId));
        });

        group.MapPost("/register", (RegisterUserInput input, ISender sender) =>
        {
            return sender.Send(input);
        });

        return app;
    }
}