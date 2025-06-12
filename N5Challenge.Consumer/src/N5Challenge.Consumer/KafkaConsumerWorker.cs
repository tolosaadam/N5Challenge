using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Confluent.Kafka.Admin;

namespace N5Challenge.Consumer;

public class KafkaConsumerBackgroundService(IOptions<Domain.KafkaSettings> kpSettings, IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly ConsumerConfig _consumerConfig = new()
    {
        BootstrapServers = kpSettings.Value.BootstrapServers,
        GroupId = "my-consumer-group", // Para balancear carga.
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var adminClient = new AdminClientBuilder(_consumerConfig).Build();

        var requiredTopics = new[] { "permissions", "permission_types" };

        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));

        var existingTopics = metadata.Topics.Select(t => t.Topic).ToHashSet();

        var topicsToCreate = requiredTopics
            .Where(t => !existingTopics.Contains(t))
            .Select(t => new TopicSpecification
            {
                Name = t,
                NumPartitions = 1,
                ReplicationFactor = 1
            })
            .ToList();

        if (topicsToCreate.Any())
        {
            try
            {
                await adminClient.CreateTopicsAsync(topicsToCreate);
                Console.WriteLine("Topics creados correctamente.");
            }
            catch (CreateTopicsException e)
            {
                foreach (var result in e.Results)
                {
                    if (result.Error.Code != ErrorCode.TopicAlreadyExists)
                        Console.WriteLine($"Error creando el topic {result.Topic}: {result.Error.Reason}");
                }
            }
        }
        else
        {
            Console.WriteLine("Todos los topics ya existen. No se creó ninguno.");
        }

        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();

        consumer.Subscribe(requiredTopics);

        Console.WriteLine($"Escuchando tópicos: {string.Join(", ", requiredTopics)}");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(cancellationToken);
                var topic = result.Topic;
                var message = result.Message.Value;

                // Evento auditable
                if (topic.EndsWith("auditable"))
                {
                    Console.WriteLine($"Auditable Event - {message} {topic}");
                    continue;
                }

                await ProcessMessageAsync(topic, message, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error procesando mensaje: {ex.Message}");
            }
        }
    }

    private async Task ProcessMessageAsync(string topic, string json, CancellationToken cancellationToken)
    {
        var entityType = Domain.Dictionaries.KafkaEntityDictionary.GetEntityTypeFromTopic(topic);
        if (entityType is null)
        {
            Console.WriteLine($"Tipo no mapeado para el tópico '{topic}'");
            return;
        }

        // Construye KafkaEvent<entityType> dinámicamente
        var kafkaEventType = typeof(Domain.KafkaEvent<>).MakeGenericType(entityType);

        // Deserializa el JSON al tipo correcto
        var envelope = JsonSerializer.Deserialize(json, kafkaEventType);
        if (envelope == null)
        {
            Console.WriteLine("No se pudo deserializar el mensaje.");
            return;
        }

        // Obtiene Operation y Payload por reflexión
        var operation = (Domain.Enums.OperationEnum)kafkaEventType.GetProperty("Operation")!.GetValue(envelope)!;
        var payload = kafkaEventType.GetProperty("Payload")!.GetValue(envelope)!;

        using var scope = _serviceProvider.CreateScope();
        var elasticService = scope.ServiceProvider.GetRequiredService<ElasticSearch.IElasticSearchService>();

        switch (operation)
        {
            case Domain.Enums.OperationEnum.request:
                await elasticService.IndexAsync(entity: (Domain.Indexables.IndexableEntity)payload, indexName: topic, cancellationToken: cancellationToken);
                break;
            case Domain.Enums.OperationEnum.modify:
                await elasticService.IndexAsync(entity: (Domain.Indexables.IndexableEntity)payload, indexName: topic, cancellationToken: cancellationToken);
                break;
            default:
                break;
        }

        await Task.CompletedTask;
    }
}
