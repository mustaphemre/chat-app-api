using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.Chats.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplicationServices(this IServiceCollection services)
    {
        // Add gRPC client that connects to UsersService
        services.AddGrpcClient<UsersService.Grpc.UsersService.UsersServiceClient>(o =>
        {
            o.Address = new Uri("https://chatapp-user-api"); // Discovered by Aspire
        });
    }
}
