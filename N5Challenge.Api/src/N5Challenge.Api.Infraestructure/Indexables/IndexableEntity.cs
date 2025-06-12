using N5Challenge.Api.Infraestructure.Entities;
using N5Challenge.Api.Infraestructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Indexables;

public class IndexableEntity : Entity<string>, IIndexableEntity
{
}
