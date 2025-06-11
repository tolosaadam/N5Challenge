using N5Challenge.Common.Infraestructure.Indexables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.ElasticSearch;

public interface IElasticSearchService
{
    Task IndexAsync(object entity, string indexName, CancellationToken cancellationToken = default);
    Task IndexAsync(IEnumerable<object> entities, string indexName, CancellationToken cancellationToken = default);
    void Index(object entity, string indexName);
    void Index(IEnumerable<object> entities, string indexName);
}
