using ChatApp.Users.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Users.Application.Users;

internal class GetUserByIdHandler : IRequestHandler<GetUserByIdInput, UserOutput>
{
    private readonly UsersDbContext _dbContext;

    public GetUserByIdHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserOutput> Handle(GetUserByIdInput request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
        if (user is null)
        {
            throw new Exception("User not found!");
        }

        return new UserOutput
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Status = user.Status,
            ProfilePicture = user.ProfilePicture
        };
    }
}