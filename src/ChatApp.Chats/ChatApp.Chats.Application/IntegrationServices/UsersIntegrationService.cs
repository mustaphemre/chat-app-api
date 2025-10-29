using ChatApp.Chats.Application.IntegrationServices.Dtos;
using static UsersService.Grpc.UsersService;

namespace ChatApp.Chats.Application.IntegrationServices;

public class UsersIntegrationService : IUsersIntegrationService
{
    private readonly UsersServiceClient _usersServiceClient;

    public UsersIntegrationService(UsersServiceClient usersServiceClient)
    {
        _usersServiceClient = usersServiceClient;
    }

    public async Task<UserOutput> GetUserAsync(GetUserInput input, CancellationToken cancellationToken)
    {
        var userDto = await _usersServiceClient.GetUserByIdAsync(
            new UsersService.Grpc.GetUserByIdRequest { UserId = input.UserId.ToString() },
            cancellationToken: cancellationToken
            );

        return new UserOutput
        {
            UserId = Guid.Parse(userDto.UserId),
            Username = userDto.Username,
            Email = userDto.Email,
            ProfilePicture = userDto.ProfilePicture,
            Status = userDto.Status,
        };
    }
}
