using ChatApp.Chats.Domain;
using Confluent.Kafka;
using System.Text.Json;

namespace ChatApp.Chats.Application.Producers;

public class ChatMessageProducer
{
    private readonly IProducer<string, string> _producer;

    public ChatMessageProducer(IProducer<string, string> producer)
    {
        _producer = producer;
    }
    public async Task PublishAsync(string key, object message)
    {
        var json = JsonSerializer.Serialize(message);

        await _producer.ProduceAsync(Constants.TopicNames.ChatTopic, new Message<string, string>
        {
            Key = key,
            Value = json
        });
    }
}
