using ChatApp.Chats.Domain;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Text.Json;

namespace ChatApp.Chats.Application.Producers;

public class ChatMessageProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<ChatMessageProducer> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public ChatMessageProducer(
        IProducer<string, string> producer,
        ILogger<ChatMessageProducer> logger)
    {
        _producer = producer;
        _logger = logger;
        _retryPolicy = ConfigureRetryPolicy();
    }

    public async Task PublishAsync(string key, object message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Publishing message to {Topic}: {Message}", Constants.TopicNames.ChatTopic, message);

        var json = JsonSerializer.Serialize(message);

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var result = await _producer.ProduceAsync(Constants.TopicNames.ChatTopic,
                new Message<string, string>
                {
                    Key = key,
                    Value = json
                },
                cancellationToken);

            _logger.LogInformation("Delivered to {TopicPartitionOffset}", result.TopicPartitionOffset);
        });
    }

    private AsyncRetryPolicy ConfigureRetryPolicy()
    {
        return Policy
            .Handle<ProduceException<string, string>>()
            .Or<KafkaException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (ex, span, attempt, ctx) =>
                {
                    _logger.LogWarning(ex,
                            "Retrying Kafka publish (attempt {Attempt}) after {Delay}s due to error: {Message}",
                            attempt, span.TotalSeconds, ex.Message);
                });
    }
}