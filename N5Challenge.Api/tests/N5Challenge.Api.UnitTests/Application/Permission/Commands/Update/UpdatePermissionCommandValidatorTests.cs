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
    [DataRow(1, true)]
    [DataRow(0, false)]
    public async Task ValidatorTest(int id, bool isValid)
    {
        // Arrange
        var validator = new UpdatePermissionCommandValidator();
        var request = new UpdatePermissionCommand(id, "Adam", "Tolosa", 1, new DateTime(default));

        // Act
        var result = await validator.ValidateAsync(request);

        // Assert
        result.IsValid
            .Should()
            .Be(isValid, $"because Id='{id}'");
    }
}
