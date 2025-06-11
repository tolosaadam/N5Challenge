using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Indexables;

public class IndexablePermissionType(string id) : IndexableEntity(id)
{
    public string? Description { get; set; }
}
