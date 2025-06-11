using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Common.Infraestructure;

public interface IEntity<TId>
{
    TId Id { get; }
}
