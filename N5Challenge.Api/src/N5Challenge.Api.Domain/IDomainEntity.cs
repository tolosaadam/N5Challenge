using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Domain;

public interface IDomainEntity<TId>
{
    TId Id { get; }
}
