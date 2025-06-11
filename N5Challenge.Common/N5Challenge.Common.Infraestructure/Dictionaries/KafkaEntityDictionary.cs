using N5Challenge.Common.Constants;
using N5Challenge.Common.Infraestructure.Indexables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure.Dictionaries;

public static class KafkaEntityDictionary
{
    private static readonly Dictionary<string, Type> TopicToEntityMap = new()
    {
        [EntityRawNameConstants.PERMISSIONS] = typeof(IndexablePermission),
        [EntityRawNameConstants.PERMISSION_TYPES] = typeof(IndexablePermissionType),
    };

    public static Type? GetEntityTypeFromTopic(string topic)
    {
        return TopicToEntityMap.TryGetValue(topic, out var type) ? type : null;
    }
}
