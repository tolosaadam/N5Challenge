using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Exceptions;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Models;
using N5Challenge.Api.Application.Permission.Commands.UpdatePartial;
using N5Challenge.Api.Domain;
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
    private readonly Mock<IPermissionRepository> _pRepositoryMock = new();
    private readonly Mock<IPermissionTypeRepository> _pTypeRepositoryMock = new();
    private readonly Mock<IMapper> _autoMapperMock = new();
    private readonly Mock<IElasticSearch> _elasticSearchMock = new();

    private UpdatePartialPermissionCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock
            .Setup(u => u.GetRepository<IPermissionRepository>())
            .Returns(_pRepositoryMock.Object);

        _unitOfWorkMock
            .Setup(u => u.GetRepository<IPermissionTypeRepository>())
            .Returns(_pTypeRepositoryMock.Object);

        _handler = new UpdatePartialPermissionCommandHandler(
            _unitOfWorkMock.Object,
            _autoMapperMock.Object,
            _elasticSearchMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ShouldUpdatePermission()
    {
        // Arrange
        var command = new UpdatePartialPermissionCommand(1, "Adam", "Tolosa", 1, new DateTime(default));

        var cancellationToken = CancellationToken.None;

        var permission = new Domain.Permission
        {
            EmployeeFirstName = "Adam",
            EmployeeLastName = "Tolosa",
            PermissionTypeId = 1
        };

        var pType = new PermissionType
        {
            Id = 1
        };

        var indexablePermission = new IndexablePermission("1");

        _pRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(permission);

        _pTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId.Value, cancellationToken))
            .ReturnsAsync(pType);

        _autoMapperMock
            .Setup(m => m.Map(command, permission))
            .Returns(permission);

        _pRepositoryMock
            .Setup(r => r.Update(permission))
            .Returns(permission);

        _autoMapperMock
            .Setup(m => m.Map<IndexablePermission>(permission))
            .Returns(indexablePermission);

        _elasticSearchMock
            .Setup(e => e.IndexAsync(indexablePermission, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, cancellationToken)!;

        // Assert
        _pRepositoryMock.Verify(r => r.GetByIdAsync(command.Id, cancellationToken), Times.Once,
            "Should retrieve existing permission by ID");

        _pTypeRepositoryMock.Verify(r => r.GetByIdAsync(command.PermissionTypeId.Value, cancellationToken), Times.Once,
            "Should retrieve permission type by ID");

        _autoMapperMock.Verify(m => m.Map(command, permission), Times.Once,
            "Should map the command to Domain.Permission");

        _pRepositoryMock.Verify(r => r.Update(It.Is<Domain.Permission>(p =>
            p.EmployeeFirstName == permission.EmployeeFirstName &&
            p.EmployeeLastName == permission.EmployeeLastName &&
            p.PermissionTypeId == permission.PermissionTypeId)), Times.Once,
            "Should update permission with correct values");

        _autoMapperMock.Verify(m => m.Map<IndexablePermission>(permission), Times.Once,
            "Should map the permission to IndexablePermission");

        _elasticSearchMock.Verify(e =>
            e.IndexAsync(indexablePermission, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken),
            Times.Once, "Should index the permission in ElasticSearch once");
    }


    [TestMethod]
    public async Task Handle_ShouldThrowRelatedEntityNotFoundException()
    {
        // Arrange
        var command = new UpdatePartialPermissionCommand(1, "Adam", "Tolosa", 1, new DateTime(default));

        var cancellationToken = CancellationToken.None;

        var permission = new Domain.Permission
        {
            EmployeeFirstName = "Adam",
            EmployeeLastName = "Tolosa",
            PermissionTypeId = 1
        };

        _pRepositoryMock
            .Setup(r => r.GetByIdAsync(command.Id, cancellationToken))
            .ReturnsAsync(permission);

        _pTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId.Value, cancellationToken))
            .ReturnsAsync((PermissionType?)null);

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

        _pTypeRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PermissionTypeId.Value, cancellationToken))
            .ReturnsAsync((PermissionType?)null);

        Func<Task> act = async () => await _handler.Handle(command, cancellationToken);

        await act.Should()
            .ThrowAsync<EntityNotFoundException>("Should throw EntityNotFoundException");
    }
}
