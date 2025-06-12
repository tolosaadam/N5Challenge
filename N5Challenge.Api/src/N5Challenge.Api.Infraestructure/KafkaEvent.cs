using N5Challenge.Api.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure;

public class KafkaEvent<T>
{
    public OperationEnum Operation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public T Payload { get; set; } = default!;
}
