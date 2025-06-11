using N5Challenge.Consumer.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Consumer.Domain.Models.Domain;

public class PermissionType : IDomainEntity<int>
{
    public int Id { get; set; }
    public string? Description { get; set; }
}
