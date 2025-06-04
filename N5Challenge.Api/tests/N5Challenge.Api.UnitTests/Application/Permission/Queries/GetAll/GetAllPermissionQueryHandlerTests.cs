using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.Api.Application.Constants;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.Models;
using N5Challenge.Api.Application.Permission.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.Permission.Queries.GetAll;

[TestClass]
public class GetAllPermissionQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IPermissionRepository> _pRepositoryMock = new();
    private readonly Mock<IMapper> _autoMapperMock = new();
    private readonly Mock<IElasticSearch> _elasticMock = new();

    private GetAllPermissionQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock
            .Setup(u => u.GetRepository<IPermissionRepository>())
            .Returns(_pRepositoryMock.Object);

        _handler = new GetAllPermissionQueryHandler(
            _unitOfWorkMock.Object,
            _autoMapperMock.Object,
            _elasticMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ShouldReturnPermissions()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var permissions = new List<Domain.Permission>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        var indexablePermissions = new List<IndexablePermission>
        {
            new("1"),
            new("2")
        };

        _pRepositoryMock
            .Setup(r => r.GetAllAsync(cancellationToken))
            .ReturnsAsync(permissions);

        _autoMapperMock
            .Setup(m => m.Map<IEnumerable<IndexablePermission>>(permissions))
            .Returns(indexablePermissions);

        _elasticMock
            .Setup(e => e.IndexAsync(indexablePermissions, IndexNamesConstans.PERMISSION_INDEX_NAME, cancellationToken))
            .Returns(Task.CompletedTask);

        var query = new GetAllPermissionQuery();

        // Act
        var result = await _handler.Handle(query, cancellationToken)!;

        // Assert
        result.Should().NotBeNull("Result should contain 2 permissions");
        result.Should().BeEquivalentTo(permissions, "Result should be equal to mocked permissions");
        result.Should().HaveCount(2, "Result should contain 2 permissions");
        result.Should().Contain(p => p.Id == 1, "Result should contain permission with Id 1");
        result.Should().Contain(p => p.Id == 2, "Result should contain permission with Id 2");
    }
}
