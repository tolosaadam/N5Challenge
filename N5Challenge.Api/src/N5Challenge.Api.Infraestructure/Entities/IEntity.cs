using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Entities;

public interface IEntity<TId>
{
    TId Id { get; }
}
