using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Exceptions;

public class EntityNotFoundException(string entityName, object key) : Exception($"{entityName} with ID '{key}' was not found.")
{
}
