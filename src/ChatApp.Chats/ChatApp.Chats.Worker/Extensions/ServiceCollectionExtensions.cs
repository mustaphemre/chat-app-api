using ChatApp.Chats.Worker.Consumers.ChatMessages;
using Confluent.Kafka;

namespace ChatApp.Chats.Worker.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConsumerServices(this IHostApplicationBuilder builder)
    {
        // Kafka connection (Aspire provides it automatically)
        var kafkaBootstrap = builder.Configuration["ConnectionStrings:kafka"];

        builder.Services.AddSingleton(new ConsumerConfig
        {
            BootstrapServers = kafkaBootstrap,
            GroupId = "chat-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        });

        builder.Services.AddHostedService<ChatMessageConsumer>();
    }
}
