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

        var envelope = JsonSerializer.Deserialize<Common.Infraestructure.KafkaEvent>(json);

        if (envelope == null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var elasticService = scope.ServiceProvider.GetRequiredService<IElasticSearchService>();

        dynamic dynEnvelope = envelope;

        switch ((Common.Enums.OperationEnum)dynEnvelope.Operation)
        {
            case Common.Enums.OperationEnum.request:
                await elasticService.IndexAsync(entity: dynEnvelope.Payload, indexName: topic, cancellationToken: cancellationToken);
                Console.WriteLine($"[CREATE] Tópico: {topic}, ID: {dynEnvelope.Payload.Id}");
                break;

            case Common.Enums.OperationEnum.modify:
                // index async (update)
                Console.WriteLine($"[UPDATE] Tópico: {topic}, ID: {dynEnvelope.Payload.Id}");
                break;

            default:
                Console.WriteLine($"[IGNORADO] Operación {dynEnvelope.Operation}");
                break;
        }

        await Task.CompletedTask;
    }
}
