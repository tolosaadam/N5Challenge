using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure;

public class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; } = default!;
}
