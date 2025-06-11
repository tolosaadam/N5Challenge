using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common;

public class KafkaSettings
{
    public string? BootstrapServers { get; set; }
    public int MessageTimeoutMs { get; set; }
    public int SocketTimeoutMs { get; set; }
}
