using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Common.Enums;
using N5Challenge.Common.Infraestructure;
using N5Challenge.Common.Infraestructure.Dictionaries;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace N5Challenge.Api.Infraestructure.Services.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducer> _logger;
    private readonly IMapper _autoMapper;
    public KafkaProducer(IOptions<KafkaSettings> kpSettings, ILogger<KafkaProducer> logger, IMapper autoMapper)
    {
        _logger = logger;
        _autoMapper = autoMapper;
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

    public async Task PublishEntityEventAsync<TDomainModel>(
        string topic,
        TDomainModel entity,
        OperationEnum operation,
        CancellationToken cancellationToken)
    {
        try
        {
            var targetType = KafkaEntityDictionary.GetEntityTypeFromTopic(topic);

            if (targetType == null)
                throw new InvalidOperationException($"No IndexableEntity type found for topic '{topic}'");

            var mapped = _autoMapper.Map(entity, entity!.GetType(), targetType);

            // Crear tipo KafkaEvent<T> dinámicamente
            var kafkaEventType = typeof(KafkaEvent<>).MakeGenericType(targetType);
            var envelope = Activator.CreateInstance(kafkaEventType);
            kafkaEventType.GetProperty("Operation")!.SetValue(envelope, operation);
            kafkaEventType.GetProperty("Payload")!.SetValue(envelope, mapped);

            // Serializar usando el tipo dinámico
            var json = JsonSerializer.Serialize(envelope, kafkaEventType, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

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