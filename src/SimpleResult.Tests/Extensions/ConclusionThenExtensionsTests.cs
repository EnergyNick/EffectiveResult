using SimpleResult.Core;
using SimpleResult.Extensions;

namespace SimpleResult.Tests.Extensions;

public class ConclusionThenExtensionsTests
{
    [Fact]
    public void ThenExtension_WhenInvokeOnSuccessResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.Then(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public void ThenExtension_WhenInvokeOnFailedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.Then(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void OnFailExtension_WhenInvokeOnFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnFail(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public void OnFailExtension_WhenInvokeOnSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnFail(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void OnFailExtension_WhenInvokeOnFailedResultWithArgumentAction_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.OnFail(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().NotBeNull().And.BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void OnFailExtension_WhenInvokeOnSuccessResultWithArgumentAction_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.OnFail(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }
}