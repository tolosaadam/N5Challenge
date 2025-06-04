using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N5Challenge.Api.Application.Permission.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.Permission.Commands.Create;

[TestClass]
public class CreatePermissionCommandValidatorTests
{
    [TestMethod]
    [DataRow("xxxx", "xxxx", 1, true)]
    [DataRow("", "xxxx", 1, false)]
    [DataRow("xxxx", null, 1, false)]
    [DataRow("xxxx", "xxxx", 0, false)]
    public async Task ValidatorTest(string firstName, string lastName, int pTypeId, bool isValid)
    {
        // Arrange
        var validator = new CreatePermissionCommandValidator();
        var request = new CreatePermissionCommand(firstName, lastName, pTypeId);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid
            .Should()
            .Be(isValid, $"because FirstName='{firstName}', LastName='{lastName}', and PermissionTypeId={pTypeId}");
    }
}