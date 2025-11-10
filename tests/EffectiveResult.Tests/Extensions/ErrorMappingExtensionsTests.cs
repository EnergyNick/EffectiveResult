using EffectiveResult.Extensions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests.Extensions;

public class ErrorMappingExtensionsTests
{
    [Fact]
    public void MapErrorsOnFailedExtension_WhenSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var valuedResult = Result.Ok("Oh my!");

        var isInvoked = false;
        var func = (IReadOnlyCollection<ResultError> errors) =>
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
        var firstError = new ResultError("Fail");
        var secondError = new ResultError("To Much errors");

        var newError = new ResultError("Ho ho ho!");
        var newErrors = new ResultError[] { newError };

        var result = Result.Fail(firstError);
        var valuedResult = Result.Fail<string>(secondError);

        var func = (IReadOnlyCollection<ResultError> errors) => newErrors;

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
