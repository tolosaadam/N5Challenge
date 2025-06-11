using N5Challenge.Common.Infraestructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure.Indexables;

public class IndexableEntity : IIndexableEntity
{
    public string Id { get; set; } = default!;
}
