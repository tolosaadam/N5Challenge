using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Domain.Configuration;

public class ElasticSearchSettings
{
    public string? Url { get; set; }
    public string? DefaultIndexName { get; set; }
}
