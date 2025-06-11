using Microsoft.Extensions.Logging;
using N5Challenge.Api.Domain.Constants;
using N5Challenge.Api.Infraestructure.Constants;
using N5Challenge.Api.Infraestructure.Entities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.ElasticSearch;

public class ElasticSearchSeeder
{
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<ElasticSearchSeeder> _logger;

    public ElasticSearchSeeder(IElasticClient elasticClient, ILogger<ElasticSearchSeeder> logger)
    {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task SeedPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var indexName = EntityRawNameConstans.PERMISSIONS;

        var exists = await _elasticClient.Indices.ExistsAsync(indexName, ct: cancellationToken);
        if (!exists.Exists)
        {
            _logger.LogInformation("El índice no existe. Creando índice y cargando datos...");

            await _elasticClient.Indices.CreateAsync(indexName, ct: cancellationToken);

            var seedData = SeedDataConstants.Permissions;

            var bulk = await _elasticClient.BulkAsync(b => b
                .Index(indexName)
                .IndexMany(seedData), cancellationToken);

            if (bulk.Errors)
            {
                _logger.LogError("Ocurrieron errores al insertar datos iniciales en ElasticSearch.");
                foreach (var item in bulk.ItemsWithErrors)
                    _logger.LogError($"Error en documento {item.Id}: {item.Error?.Reason}");
            }
            else
            {
                _logger.LogInformation("Datos iniciales insertados exitosamente en ElasticSearch.");
            }
        }
        else
        {
            _logger.LogInformation("El índice ya existe. Saltando carga inicial.");
        }
    }

    public async Task SeedPermissionTypesAsync(CancellationToken cancellationToken = default)
    {
        var indexName = EntityRawNameConstans.PERMISSION_TYPES;

        var exists = await _elasticClient.Indices.ExistsAsync(indexName, ct: cancellationToken);
        if (!exists.Exists)
        {
            _logger.LogInformation("El índice no existe. Creando índice y cargando datos...");

            await _elasticClient.Indices.CreateAsync(indexName, ct: cancellationToken);

            var seedData = SeedDataConstants.PermissionTypes;

            var bulk = await _elasticClient.BulkAsync(b => b
                .Index(indexName)
                .IndexMany(seedData), cancellationToken);

            if (bulk.Errors)
            {
                _logger.LogError("Ocurrieron errores al insertar datos iniciales en ElasticSearch.");
                foreach (var item in bulk.ItemsWithErrors)
                    _logger.LogError($"Error en documento {item.Id}: {item.Error?.Reason}");
            }
            else
            {
                _logger.LogInformation("Datos iniciales insertados exitosamente en ElasticSearch.");
            }
        }
        else
        {
            _logger.LogInformation("El índice ya existe. Saltando carga inicial.");
        }
    }
}
