using ChatApp.Chats.Consumers.ChatMessages;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatApp.Chats.Consumers.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChatConsumerServices(this IHostApplicationBuilder builder)
    {
        // Kafka connection (Aspire provides it automatically)
        var kafkaBootstrap = builder.Configuration["KAFKA:CONNECTIONSTRING"];

        builder.Services.AddSingleton(new ConsumerConfig
        {
            BootstrapServers = kafkaBootstrap,
            GroupId = "chat-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        });

        builder.Services.AddHostedService<SendChatMessageConsumer>();
    }
}
