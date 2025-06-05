using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Exceptions;

public sealed class RelatedEntityNotFoundException(string entityName, string relatedEntityName, object relatedKey)
    : Exception($"Entity '{entityName}' refers to non-existent {relatedEntityName} with ID '{relatedKey}'.")
{
    public string? EntityName { get; } = entityName;
    public string? RelatedEntityName { get; } = relatedEntityName;
    public object? RelatedKey { get; } = relatedKey;
}
