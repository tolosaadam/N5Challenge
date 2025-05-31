using AutoMapper;
using MediatR;
using N5Challenge.Api.Application.Interfaces;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Permission.Commands.Create;

public record CreatePermissionCommand(
    string EmployeeFirstName,
    string EmployeeLastName,
    int PermissionTypeId) : IRequest<int>;

public class CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper autoMapper) : IRequestHandler<CreatePermissionCommand, int>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _autoMapper = autoMapper;

    public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var pRepository = _unitOfWork.GetRepository<IPermissionRepository>();
            
            var pDomain = _autoMapper.Map<Domain.Permission>(request);

            pDomain.Date = DateTime.UtcNow;

            await pRepository.AddAsync(pDomain, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return pDomain.Id;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
