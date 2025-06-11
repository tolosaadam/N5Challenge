using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.ElasticSearch;

public interface IElasticSearchService
{
    Task IndexAsync(IIndexableEntity entity, string indexName, CancellationToken cancellationToken = default);
    Task IndexAsync(IEnumerable<IIndexableEntity> entities, string indexName, CancellationToken cancellationToken = default);
    void Index(IIndexableEntity entity, string indexName);
    void Index(IEnumerable<IIndexableEntity> entities, string indexName);
}
