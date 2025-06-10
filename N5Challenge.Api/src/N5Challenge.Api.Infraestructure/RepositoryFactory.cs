using Microsoft.Extensions.DependencyInjection;
using N5Challenge.Api.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure;

public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly Dictionary<Type, IEfRepository> _cache = [];

    public TRepository GetEfRepository<TRepository>()
    where TRepository : IEfRepository
    {
        var type = typeof(TRepository);
        if (!_cache.TryGetValue(type, out var repo))
        {
            repo = _serviceProvider.GetRequiredService<TRepository>();
            _cache[type] = repo!;
        }
        return (TRepository)repo!;
    }
}
