using SimpleResult.Core;
using SimpleResult.Extensions;
using SimpleResult.TestsCommon.Helpers;

namespace SimpleResult.Tests.Extensions;

public class ErrorMappingExtensionsTests
{
    [Fact]
    public void MapErrorsOnFailedExtension_WhenSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var valuedResult = Result.Ok("Oh my!");

        var isInvoked = false;
        var func = (IReadOnlyCollection<IError> errors) =>
        {
            isInvoked = true;
            return errors;
        };

        // Act
        var actResult = result.MapErrorsOnFailed(func);
        var actValueResult = valuedResult.MapErrorsOnFailed(func);

        // Assert
        actResult.Should().Be(result);
        actValueResult.Should().Be(valuedResult);
        isInvoked.Should().BeFalse();
    }

    [Fact]
    public void MapErrorsOnFailedExtension_WhenFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var firstError = new Error("Fail");
        var secondError = new Error("To Much errors");

        var newError = new Error("Ho ho ho!");
        var newErrors = new IError[] { newError };

        var result = Result.Fail(firstError);
        var valuedResult = Result.Fail<string>(secondError);

        var func = (IReadOnlyCollection<IError> errors) => newErrors;

        // Act
        var actResult = result.MapErrorsOnFailed(func);
        var actValueResult = valuedResult.MapErrorsOnFailed(func);

        // Assert
        actResult.Should().NotBe(result);
        actValueResult.Should().NotBe(valuedResult);
        actResult.ShouldBeFailed(newErrors);
        actValueResult.ShouldBeFailed(newErrors);
    }
}