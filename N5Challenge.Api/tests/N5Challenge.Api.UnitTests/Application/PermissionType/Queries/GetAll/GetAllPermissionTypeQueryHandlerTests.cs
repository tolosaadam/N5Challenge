﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using N5Challenge.Api.Application.Interfaces.Persistence;
using N5Challenge.Api.Application.PermissionType.Queries.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.PermissionType.Queries.GetAll;

[TestClass]
public class GetAllPermissionTypeQueryHandlerTests
{
    private readonly Mock<IElasticPermissionTypeRepository> _elasticPtRepositoryMock = new();

    private GetAllPermissionTypeQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {

        _handler = new GetAllPermissionTypeQueryHandler(
            _elasticPtRepositoryMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ShouldReturnPermissionTypes()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        var permissionTypes = new List<Domain.PermissionType>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        _elasticPtRepositoryMock
            .Setup(r => r.GetAllAsync(cancellationToken))  
            .ReturnsAsync(permissionTypes);

        var query = new GetAllPermissionTypeQuery();

        // Act
        var result = await _handler.Handle(query, cancellationToken)!;

        // Assert
        result.Should().NotBeNull("Result should not be null");
        result.Should().BeEquivalentTo(permissionTypes, "Result should be equal to mocked permission types");
        result.Should().HaveCount(2, "Result should contain 2 permission types");
        result.Should().Contain(p => p.Id == 1, "Result should contain permission type with Id 1");
        result.Should().Contain(p => p.Id == 2, "Result should contain permission type with Id 2");
    }
}

