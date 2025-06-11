using N5Challenge.Consumer.Domain.Models.Enums;
using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models;

public class KafkaEnvelope<TDomainModel, TId>
    where TDomainModel : class, IDomainEntity<TId>
{
    public OperationEnum Operation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public TDomainModel Payload { get; set; } = default!;
}
