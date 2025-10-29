using ChatApp.Chats.Application.IntegrationServices;
using ChatApp.Chats.Application.Producers;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApp.Chats.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatApplicationServices(this IHostApplicationBuilder builder)
    {
        AddGrpcClients(builder);
        AddProducerConfigs(builder);

        builder.Services.AddTransient<IUsersIntegrationService, UsersIntegrationService>();
    }

    private static void AddGrpcClients(IHostApplicationBuilder builder)
    {
        // Add gRPC client that connects to UsersService
        builder.Services.AddGrpcClient<UsersService.Grpc.UsersService.UsersServiceClient>(o =>
        {
            o.Address = new Uri("https://chatapp-user-api"); // Discovered by Aspire
        });
    }

    private static void AddProducerConfigs(IHostApplicationBuilder builder)
    {
        // Kafka connection (Aspire provides it automatically)
        var kafkaBootstrap = builder.Configuration["ConnectionStrings:kafka"];

        builder.Services.AddSingleton(new ProducerConfig
        {
            BootstrapServers = kafkaBootstrap
        });

        builder.Services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<ProducerConfig>();
            return new ProducerBuilder<string, string>(config).Build();
        });

        builder.Services.AddSingleton<ChatMessageProducer>();
    }
}