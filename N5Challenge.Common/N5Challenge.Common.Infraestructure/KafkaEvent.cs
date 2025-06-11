using N5Challenge.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure;

public class KafkaEvent
{
    public OperationEnum Operation { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public object Payload { get; set; } = default!;
}
