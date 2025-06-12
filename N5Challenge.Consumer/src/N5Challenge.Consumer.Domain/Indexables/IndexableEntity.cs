using N5Challenge.Consumer.Domain;
using N5Challenge.Consumer.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Indexables;

public class IndexableEntity : Entity<string>, IIndexableEntity
{
}
