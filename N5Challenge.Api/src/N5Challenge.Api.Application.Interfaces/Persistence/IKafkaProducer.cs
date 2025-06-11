using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IKafkaProducer
{
    Task PublishAuditableEventAsync(string topic, OperationEnum operation, CancellationToken cancellationToken = default);
    Task PublishEventAsync<TDomainModel>(string topic, TDomainModel entity, OperationEnum operation, CancellationToken cancellationToken = default);
}
