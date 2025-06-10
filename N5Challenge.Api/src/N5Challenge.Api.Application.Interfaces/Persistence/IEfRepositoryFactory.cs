using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Interfaces.Persistence;

public interface IEfRepositoryFactory
{
    TRepository GetEfRepository<TRepository>()
    where TRepository : IEfRepository;
}