using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Domain.Enums;
using System.Runtime;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    public async Task PublishAuditableEventAsync(string topic, OperationEnum operation, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _producer.ProduceAsync($"{topic}.auditable", new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = operation.ToString()
            },
            cancellationToken);

            _logger.LogInformation("Success message send. Topic: {@Topic}, Operation: {@Operation}", topic, operation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message. Topic: {@Topic}, Operation: {@Operation}", topic, operation);
        }
    }

    public async Task PublishEventAsync<TDomainModel>(
        string topic,
        TDomainModel entity,
        OperationEnum operation,
        CancellationToken cancellationToken)
    {
        try
        {
            var envelope = new KafkaEnvelope<TDomainModel>
            {
                Operation = operation,
                Payload = entity
            };

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var json = JsonSerializer.Serialize(envelope, options);

            var result = await _producer.ProduceAsync(topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            },
            cancellationToken);

            _logger.LogInformation("Success message send. Topic: {@Topic}, Entity: {@Entity} Operation: {@Operation}", topic, entity, operation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message. Topic: {@Topic}, Operation: {Operation}", topic, operation);
        }
    }
}