using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using N5Challenge.Api.Application.Permission.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.UnitTests.Application.Permission.Commands.Update;

[TestClass]
public class UpdatePermissionCommandValidatorTests
{
    [TestMethod]
    [DataRow(1, "xxxx", "xxxx", 1, true)]
    [DataRow(0, "xxxx", "xxxx", 1, false)]
    [DataRow(1, "", "xxxx", 1, false)]
    [DataRow(1, "xxxx", null, 1, false)]
    [DataRow(1, "xxxx", "xxxx", 0, false)]
    public async Task ValidatorTest(int id, string firstName, string lastName, int pTypeId, bool isValid)
    {
        // Arrange
        var validator = new UpdatePermissionCommandValidator();
        var request = new UpdatePermissionCommand(id, firstName, lastName, pTypeId, DateTime.UtcNow);

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid
            .Should()
            .Be(isValid, $"because FirstName='{firstName}', LastName='{lastName}', and PermissionTypeId={pTypeId}");
    }
}
