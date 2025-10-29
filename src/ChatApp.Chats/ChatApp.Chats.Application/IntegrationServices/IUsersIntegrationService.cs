using ChatApp.Chats.Application.IntegrationServices.Dtos;

namespace ChatApp.Chats.Application.IntegrationServices;

public interface IUsersIntegrationService
{
    Task<UserOutput> GetUserAsync(GetUserInput input, CancellationToken cancellationToken);
}
