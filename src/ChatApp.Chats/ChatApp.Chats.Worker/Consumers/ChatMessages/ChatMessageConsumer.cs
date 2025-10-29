using ChatApp.Chats.Domain;
using ChatApp.Chats.Domain.EventModels;
using Confluent.Kafka;
using System.Text.Json;

namespace ChatApp.Chats.Worker.Consumers.ChatMessages;

internal class ChatMessageConsumer : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<ChatMessageConsumer> _logger;

    public ChatMessageConsumer(
        ConsumerConfig config,
        ILogger<ChatMessageConsumer> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe(Constants.TopicNames.ChatTopic);

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
            catch (OperationCanceledException)
            {
                consumer.Close();
            }

            await Task.Delay(200, stoppingToken); // avoid tight loop
        }
    }
}
