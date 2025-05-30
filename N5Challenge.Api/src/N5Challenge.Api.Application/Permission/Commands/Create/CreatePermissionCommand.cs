using MediatR;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Create;

public record CreatePermissionCommand(Domain.Permission Permission) : IRequest<int>;

public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, int>
{
    public CreatePermissionCommandHandler()
    {

    }

    public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
