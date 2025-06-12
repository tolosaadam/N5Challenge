using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Commands.UpdatePartial;
using N5Challenge.Api.Domain;
using N5Challenge.Api.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.Permission.Commands.UpdatePartial;

[TestClass]
public class UpdatePartialPermissionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IEfPermissionRepository> _pRepositoryMock = new();
    private readonly Mock<IElasticPermissionTypeRepository> _elasticPermissionTypeRepositoryMock = new();
    private readonly Mock<IElasticPermissionRepository> _elasticPermissionRepositoryMock = new();
    private readonly Mock<IMapper> _autoMapperMock = new();
    private readonly Mock<IKafkaProducer> _kafkaProducerMock = new();

    private UpdatePartialPermissionCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock
            .Setup(u => u.GetEfRepository<IEfPermissionRepository>())
            .Returns(_pRepositoryMock.Object);

        _handler = new UpdatePartialPermissionCommandHandler(
            _unitOfWorkMock.Object,
            _autoMapperMock.Object,
            _elasticPermissionRepositoryMock.Object,
            _elasticPermissionTypeRepositoryMock.Object,
            _kafkaProducerMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ShouldUpdatePermission()
    {
        // Arrange
        var command = new UpdatePartialPermissionCommand(1, "Adam", "Tolosa", 1, new DateTime(default));

        var cancellationToken = CancellationToken.None;

        var pType = new Domain.PermissionType
        {
            Id = 1
        };

        var permission = new Domain.Permission
        {
            EmployeeFirstName = "Adam",
            EmployeeLastName = "Tolosa",
            Type = pType
        };

        _elasticPermissionRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(permission);

        _elasticPermissionTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId!.Value, cancellationToken))
            .ReturnsAsync(pType);

        _autoMapperMock
            .Setup(m => m.Map(command, permission))
            .Returns(permission);

        _pRepositoryMock
            .Setup(r => r.Update(permission))
            .Returns(permission);

        _kafkaProducerMock
            .Setup(e => e.PublishEntityEventAsync(EntityRawNameConstants.PERMISSIONS, permission, Domain.Enums.OperationEnum.modify, cancellationToken))
            .Returns(Task.CompletedTask);

        _kafkaProducerMock
            .Setup(e => e.PublishAuditableEventAsync(EntityRawNameConstants.PERMISSIONS, Domain.Enums.OperationEnum.modify, cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken)!;

        // Assert
        _elasticPermissionRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, cancellationToken), Times.Once,
            "Should retrieve existing permission by ID");

        _elasticPermissionTypeRepositoryMock.Verify(r => r.GetByIdAsync(command.PermissionTypeId!.Value, cancellationToken), Times.Once,
            "Should retrieve permission type by ID");

        _autoMapperMock.Verify(m => m.Map(command, permission), Times.Once,
            "Should map the command to Domain.Permission");

        _pRepositoryMock.Verify(r => r.Update(It.Is<Domain.Permission>(p =>
            p.EmployeeFirstName == permission.EmployeeFirstName &&
            p.EmployeeLastName == permission.EmployeeLastName &&
            p.Type == permission.Type)), Times.Once,
            "Should update permission with correct values");

        _kafkaProducerMock.Verify(e =>
            e.PublishEntityEventAsync(EntityRawNameConstants.PERMISSIONS, permission, Domain.Enums.OperationEnum.modify, cancellationToken),
            Times.Once, "Permission should be indexed in Elastic once");
    }


    [TestMethod]
    public async Task Handle_ShouldThrowRelatedEntityNotFoundException()
    {
        // Arrange
        var command = new UpdatePartialPermissionCommand(1, "Adam", "Tolosa", 1, new DateTime(default));

        var cancellationToken = CancellationToken.None;

        var pType = new Domain.PermissionType
        {
            Id = 1
        };

        var permission = new Domain.Permission
        {
            EmployeeFirstName = "Adam",
            EmployeeLastName = "Tolosa",
            Type = pType
        };

        _elasticPermissionRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(permission);

        _elasticPermissionTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId!.Value, cancellationToken))
            .ReturnsAsync((Domain.PermissionType?)null);

        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        await act.Should()
            .ThrowAsync<RelatedEntityNotFoundException>("Should throw RelatedEntityNotFoundException");
    }

    [TestMethod]
    public async Task Handle_ShouldThrowEntityNotFoundException()
    {
        // Arrange
        var command = new UpdatePartialPermissionCommand(1, "Adam", "Tolosa", 1, new DateTime(default));

        var cancellationToken = CancellationToken.None;

        _elasticPermissionTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId!.Value, cancellationToken))
            .ReturnsAsync((Domain.PermissionType?)null);

        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        await act.Should()
            .ThrowAsync<EntityNotFoundException>("Should throw EntityNotFoundException");
    }
}
