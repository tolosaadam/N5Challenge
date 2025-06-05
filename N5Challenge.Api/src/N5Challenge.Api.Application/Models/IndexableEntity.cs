using N5Challenge.Api.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Models;

public class IndexableEntity : IIndexableEntity
{
    public string Id { get; }

    public IndexableEntity(string id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
}
