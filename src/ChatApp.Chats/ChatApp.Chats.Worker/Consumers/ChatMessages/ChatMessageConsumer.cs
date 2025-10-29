using ChatApp.Chats.Domain;
using ChatApp.Chats.Domain.EventModels;
using Confluent.Kafka;
using Polly.Retry;
using System.Text.Json;

namespace ChatApp.Chats.Worker.Consumers.ChatMessages;

internal class ChatMessageConsumer : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly ILogger<ChatMessageConsumer> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public ChatMessageConsumer(
        ConsumerConfig config,
        ILogger<ChatMessageConsumer> logger)
    {
        _config = config;
        _logger = logger;
        _retryPolicy = RetryPolicyConfiguration.Build(logger);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe(Constants.TopicNames.ChatTopic);

        _logger.LogInformation("Kafka consumer started. Listening on topic: {Topic}", Constants.TopicNames.ChatTopic);

        await _retryPolicy.ExecuteAsync(async (ct) =>
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(ct);

                    _logger.LogInformation("Received message: {Value}", cr.Message.Value);

                    var data = JsonSerializer.Deserialize<ChatMessageSendEvent>(cr.Message.Value);
                    if (data is not null)
                    {
                        _logger.LogInformation("Message consumed. Chat id: {0}, message: {1}", data.ChatId, data.Content);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Kafka consume exception: {Reason}", ex.Error.Reason);
                    throw;
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }

                await Task.Delay(200, ct); // avoid tight loop
            }
        }, cancellationToken);
    }
}