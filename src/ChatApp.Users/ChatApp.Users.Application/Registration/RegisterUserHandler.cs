using ChatApp.Users.Domain.Users;
using ChatApp.Users.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Users.Application.Registration;

public class RegisterUserHandler : IRequestHandler<RegisterUserInput, RegisterUserOutput>
{
    private readonly UsersDbContext _dbContext;

    public RegisterUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RegisterUserOutput> Handle(RegisterUserInput request, CancellationToken cancellationToken)
    {
        var existedUser = await _dbContext.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);
        if (existedUser)
        {
            throw new Exception("username already taken!");
        }

        var user = new User(
            Guid.NewGuid(),
            request.Username,
            request.Email,
            "1",
            request.ProfilePicture ?? "");

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterUserOutput
        {
            Success = true,
            UserId = user.Id,
        };
    }
}
