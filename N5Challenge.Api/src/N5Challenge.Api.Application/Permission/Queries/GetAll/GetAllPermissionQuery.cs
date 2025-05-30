using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Queries.GetAll;

public record GetAllPermissionQuery() : IRequest<IEnumerable<Domain.Permission>>;

public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, IEnumerable<Domain.Permission>>
{
    public GetAllPermissionQueryHandler()
    {

    }

    public async Task<IEnumerable<Domain.Permission>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        
        return new List<Domain.Permission>();
    }
}