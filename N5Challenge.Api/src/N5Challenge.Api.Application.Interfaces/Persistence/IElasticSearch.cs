using N5Challenge.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IElasticSearch
{
    Task IndexAsync(IIndexableEntity entity, string indexName, CancellationToken cancellationToken = default);

    Task IndexAsync(IEnumerable<IIndexableEntity> entities, string indexName, CancellationToken cancellationToken = default);

    void Index(IIndexableEntity entity, string indexName);

    void Index(IEnumerable<IIndexableEntity> entities, string indexName);
}
