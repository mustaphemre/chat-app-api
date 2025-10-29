using Confluent.Kafka;
using Polly;
using Polly.Retry;

namespace ChatApp.Chats.Worker.Consumers;

internal static class RetryPolicyConfiguration
{
    public static AsyncRetryPolicy Build(ILogger logger)
    {
        return Policy
            .Handle<KafkaException>()
            .Or<ConsumeException>()
            .WaitAndRetryForeverAsync(
                retryAttempt => TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryAttempt))),
                (ex, span) =>
                {
                    logger.LogWarning(ex,
                        "Kafka consume failed. Retrying in {Delay}s. Error: {Message}",
                        span.TotalSeconds, ex.Message);
                });
    }
}
