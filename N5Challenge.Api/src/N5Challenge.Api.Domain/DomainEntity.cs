﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Domain;

public class DomainEntity<TId> : IDomainEntity<TId>
{
    public TId Id { get; set; } = default!;
}
