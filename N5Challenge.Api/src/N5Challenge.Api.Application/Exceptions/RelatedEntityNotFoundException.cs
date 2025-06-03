using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Exceptions;

public class RelatedEntityNotFoundException(string entityName, string relatedEntityName, object relatedKey) : Exception($"Entity '{entityName}' refers to non-existent {relatedEntityName} with ID '{relatedKey}'.")
{
}
