using N5Challenge.Consumer.Domain.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Dictionaries;

public static class KafkaEntityDictionary
{
    public static readonly Dictionary<string, Type> TopicToEntityMap = new()
    {
        ["permissions"] = typeof(Permission),
        ["permission_types"] = typeof(PermissionType),
    };
}
