using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IElasticSearch<TIndexableEntity>
{
    Task IndexAsync(TIndexableEntity entity, string indexName, CancellationToken cancellationToken = default);

    Task IndexAsync(IEnumerable<TIndexableEntity> entities, string indexName, CancellationToken cancellationToken = default);

    void Index(TIndexableEntity entity, string indexName);

    void Index(IEnumerable<TIndexableEntity> entities, string indexName);
}
