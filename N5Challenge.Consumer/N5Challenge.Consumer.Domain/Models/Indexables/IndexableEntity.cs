using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Indexables;

public class IndexableEntity(string id) : IIndexableEntity
{
    public string Id { get; } = id ?? throw new ArgumentNullException(nameof(id));
}