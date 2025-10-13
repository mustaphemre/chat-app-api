using ChatApp.Chats.Domain.EventModels;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ChatApp.Chats.Consumers.ChatMessages;

internal class SendChatMessageConsumer : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<SendChatMessageConsumer> _logger;

    public SendChatMessageConsumer(
        ConsumerConfig config,
        ILogger<SendChatMessageConsumer> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe("chats-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var cr = consumer.Consume(stoppingToken);

                var data = JsonSerializer.Deserialize<ChatMessageSendEvent>(cr.Message.Value);
                if (data is not null)
                {
                    _logger.LogInformation("Message consumed. Chat id: {0}, message: {1}", data.ChatId, data.Content);
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Kafka error: {e.Error.Reason}");
            }

            await Task.Delay(200, stoppingToken); // avoid tight loop
        }
    }
}
