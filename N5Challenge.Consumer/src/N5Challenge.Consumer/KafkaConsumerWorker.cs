using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json;
using N5Challenge.Consumer.ElasticSearch;
using Microsoft.Extensions.Options;
using N5Challenge.Common.Infraestructure.Dictionaries;
using N5Challenge.Common.Infraestructure.Indexables;
using N5Challenge.Common.Enums;
using N5Challenge.Common.Infraestructure;

namespace N5Challenge.Consumer;

public class KafkaConsumerBackgroundService(IOptions<Common.KafkaSettings> kpSettings, IServiceProvider serviceProvider) : BackgroundService
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
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        var topics = metadata.Topics.Select(t => t.Topic).ToList();

        using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
        consumer.Subscribe(topics);

        Console.WriteLine($"Escuchando tópicos: {string.Join(", ", topics)}");

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
        var entityType = KafkaEntityDictionary.GetEntityTypeFromTopic(topic);
        if (entityType is null)
        {
            Console.WriteLine($"Tipo no mapeado para el tópico '{topic}'");
            return;
        }

        // Construye KafkaEvent<entityType> dinámicamente
        var kafkaEventType = typeof(KafkaEvent<>).MakeGenericType(entityType);

        // Deserializa el JSON al tipo correcto
        var envelope = JsonSerializer.Deserialize(json, kafkaEventType);
        if (envelope == null)
        {
            Console.WriteLine("No se pudo deserializar el mensaje.");
            return;
        }

        // Obtiene Operation y Payload por reflexión
        var operation = (OperationEnum)kafkaEventType.GetProperty("Operation")!.GetValue(envelope)!;
        var payload = kafkaEventType.GetProperty("Payload")!.GetValue(envelope)!;

        using var scope = _serviceProvider.CreateScope();
        var elasticService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

        switch (operation)
        {
            case OperationEnum.request:
                await elasticService.IndexAsync(entity: (IndexableEntity)payload, indexName: topic, cancellationToken: cancellationToken);
                break;

            case OperationEnum.modify:
                await elasticService.IndexAsync(entity: (IndexableEntity)payload, indexName: topic, cancellationToken: cancellationToken);
                break;
            default:
                break;
        }

        await Task.CompletedTask;
    }
}
