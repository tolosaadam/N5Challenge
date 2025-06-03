using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Domain.Enums;
using System.Runtime;
using System.Text.Json;

namespace N5Challenge.Api.Infraestructure.Services.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;

    public KafkaProducer(IOptions<KafkaSettings> kpSettings, ILogger<KafkaProducer> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = kpSettings.Value.BootstrapServers,
            MessageTimeoutMs = kpSettings.Value.MessageTimeoutMs,
            SocketTimeoutMs = kpSettings.Value.SocketTimeoutMs
        };

        _producer = new ProducerBuilder<string, string>(config)
            .SetKeySerializer(Serializers.Utf8)
            .SetValueSerializer(Serializers.Utf8)
            .Build();
    }

    public async Task SendMessageAsync(string topic, OperationEnum operation, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = operation.ToString()
            },
            cancellationToken);

            _logger.LogInformation("Success message send. Entity: {@Topic}, Index: {Operation}", topic, operation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message. Entity: {@Topic}, Index: {Operation}", topic, operation);
        }
    }
}