using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Permission.Commands.Create;
using N5Challenge.Api.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.Permission.Commands.Create;

[TestClass]
public class CreatePermissionCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IEfPermissionRepository> _pRepositoryMock = new();
    private readonly Mock<IMapper> _autoMapperMock = new();
    private readonly Mock<IElasticPermissionTypeRepository> _elasticPermissionTypeRepositoryMock = new();
    private readonly Mock<IElasticPermissionRepository> _elasticPermissionRepositoryMock = new();
    private readonly Mock<IKafkaProducer> _kafkaProducerMock = new();
    private CreatePermissionCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock
            .Setup(u => u.GetEfRepository<IEfPermissionRepository>())
            .Returns(_pRepositoryMock.Object);


        _handler = new CreatePermissionCommandHandler(
            _unitOfWorkMock.Object,
            _autoMapperMock.Object,
            _elasticPermissionTypeRepositoryMock.Object,
            _elasticPermissionRepositoryMock.Object,
            _kafkaProducerMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ShouldCreatePermission()
    {
        // Arrange
        var command = new CreatePermissionCommand("Adam", "Tolosa", 1);

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

        static int idFunc() => 1;


        _elasticPermissionTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId, cancellationToken))
            .ReturnsAsync(pType);

        _autoMapperMock
            .Setup(m => m.Map<Domain.Permission>(command))
            .Returns(permission);

        _pRepositoryMock
            .Setup(r => r.AddAsync(permission, cancellationToken))
            .ReturnsAsync(idFunc);

        _kafkaProducerMock
            .Setup(e => e.PublishEntityEventAsync(EntityRawNameConstants.PERMISSIONS, permission, Domain.Enums.OperationEnum.request, cancellationToken))
            .Returns(Task.CompletedTask);

        _kafkaProducerMock
            .Setup(e => e.PublishAuditableEventAsync(EntityRawNameConstants.PERMISSIONS, Domain.Enums.OperationEnum.request, cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, cancellationToken)!;

        // Assert
        result.Should().BeGreaterThan(0, "The created permission ID should be greater than 0");

        _pRepositoryMock.Verify(r => r.AddAsync(
            It.Is<Domain.Permission>(p =>
                p.EmployeeFirstName == permission.EmployeeFirstName &&
                p.EmployeeLastName == permission.EmployeeLastName &&
                p.Type == permission.Type),
            cancellationToken), Times.Once, "Permission should be added once");

        _kafkaProducerMock.Verify(e =>
            e.PublishEntityEventAsync(EntityRawNameConstants.PERMISSIONS, permission, Domain.Enums.OperationEnum.request, cancellationToken),
            Times.Once, "Permission should be indexed in Elastic once");
    }


    [TestMethod]
    public async Task Handle_ShouldThrowRelatedEntityNotFoundException()
    {
        // Arrange
        var command = new CreatePermissionCommand("Adam", "Tolosa", 1);

        var cancellationToken = CancellationToken.None;

        var pType = new Domain.PermissionType
        {
            Id = 1
        };

        _elasticPermissionTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId, cancellationToken))
            .ReturnsAsync((Domain.PermissionType?)null);

        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        await act.Should()
            .ThrowAsync<RelatedEntityNotFoundException>();
    }
}
