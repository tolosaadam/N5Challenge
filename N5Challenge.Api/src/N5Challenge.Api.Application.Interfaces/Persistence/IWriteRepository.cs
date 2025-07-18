﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IWriteRepository<TDomainModel, TDomainId>
{
    Func<TDomainId> Add(TDomainModel domainModel);
    Task<Func<TDomainId>> AddAsync(TDomainModel domainModel, CancellationToken cancellationToken = default);
    TDomainModel? Update(TDomainModel domainModel);
    void Delete(TDomainModel domainModel);
}
