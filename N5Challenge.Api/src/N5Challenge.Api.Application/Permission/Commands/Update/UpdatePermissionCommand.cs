using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Update;

public record UpdatePermissionCommand(
    int Id,
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId,
    DateTime Date) : IRequest;

public class UpdatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper autoMapper) : IRequestHandler<UpdatePermissionCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;

    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var pRepository = _unitOfWork.GetRepository<IPermissionRepository>();            

            var pEntity = await pRepository.GetByIdAsync(request.Id, cancellationToken);

            if (pEntity is null)
            {
                throw new InvalidOperationException("Entity not found");
            }

            var pDomain = _autoMapper.Map<UpdatePermissionCommand, Domain.Permission>(request);

            pRepository.Update(pDomain);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}