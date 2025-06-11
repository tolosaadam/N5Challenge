using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.ElasticSearch;

public interface IElasticSearchService
{
    Task IndexAsync(Common.Infraestructure.Interfaces.IIndexableEntity entity, string indexName, CancellationToken cancellationToken = default);
    Task IndexAsync(IEnumerable<Common.Infraestructure.Interfaces.IIndexableEntity> entities, string indexName, CancellationToken cancellationToken = default);
    void Index(Common.Infraestructure.Interfaces.IIndexableEntity entity, string indexName);
    void Index(IEnumerable<Common.Infraestructure.Interfaces.IIndexableEntity> entities, string indexName);
}
