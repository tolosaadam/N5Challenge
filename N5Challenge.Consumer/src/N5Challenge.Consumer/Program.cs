using Microsoft.Extensions.Configuration;
using N5Challenge.Consumer;
using N5Challenge.Consumer.Domain.Models.Config;
using N5Challenge.Consumer.ElasticSearch;
using Nest;

var builder = Host.CreateApplicationBuilder(args);

var settings = new ConnectionSettings(new Uri(builder.Configuration["ElasticSearch:Url"]!))
.DisableDirectStreaming()
.EnableApiVersioningHeader();

var elasticClient = new ElasticClient(settings);

builder.Services.AddHostedService<KafkaConsumerBackgroundService>();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<IElasticClient>(elasticClient);
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();


var host = builder.Build();
host.Run();
