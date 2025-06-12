using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain;

public interface IEntity<TId>
{
    TId Id { get; }
}
