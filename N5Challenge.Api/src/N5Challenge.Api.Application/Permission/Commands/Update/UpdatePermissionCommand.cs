using MediatR;
using N5Challenge.Api.Application.Permission.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Update;

public record UpdatePermissionCommand(int Id, Domain.Permission Permission) : IRequest;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand>
{
    public UpdatePermissionCommandHandler()
    {

    }

    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}