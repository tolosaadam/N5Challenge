using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System.Text.Json;
using N5Challenge.Consumer.ElasticSearch;
using N5Challenge.Consumer.Domain.Models;
using N5Challenge.Consumer.Domain.Models.Enums;
using N5Challenge.Consumer.Domain.Models.Dictionaries;
using Microsoft.Extensions.Options;
using N5Challenge.Consumer.Domain.Models.Config;

namespace N5Challenge.Consumer;

public class KafkaConsumerBackgroundService(IOptions<KafkaSettings> kpSettings, IServiceProvider serviceProvider) : BackgroundService
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
        var entityType = GetEntityTypeFromTopic(topic);
        if (entityType is null)
        {
            Console.WriteLine($"Tipo no mapeado para el tópico '{topic}'");
            return;
        }

        var envelopeType = typeof(KafkaEnvelope<,>).MakeGenericType(entityType, typeof(int));
        var envelope = JsonSerializer.Deserialize(json, envelopeType);
        if (envelope == null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var elasticService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

        dynamic dynEnvelope = envelope;

        switch ((OperationEnum)dynEnvelope.Operation)
        {
            case OperationEnum.request:
                await elasticService.IndexAsync(entity: dynEnvelope.Payload, indexName: topic, cancellationToken: cancellationToken);
                Console.WriteLine($"[CREATE] Tópico: {topic}, ID: {dynEnvelope.Payload.Id}");
                break;

            case OperationEnum.modify:
                // index async (update)
                Console.WriteLine($"[UPDATE] Tópico: {topic}, ID: {dynEnvelope.Payload.Id}");
                break;

            default:
                Console.WriteLine($"[IGNORADO] Operación {dynEnvelope.Operation}");
                break;
        }

        await Task.CompletedTask;
    }

    private static Type? GetEntityTypeFromTopic(string topic)
    {
        return KafkaEntityDictionary.TopicToEntityMap.TryGetValue(topic, out var type) ? type : null;
    }
}
