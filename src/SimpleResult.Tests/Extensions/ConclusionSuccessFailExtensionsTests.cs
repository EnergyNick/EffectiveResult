using SimpleResult.Core;
using SimpleResult.Extensions;

namespace SimpleResult.Tests.Extensions;

public class ConclusionSuccessFailExtensionsTests
{
    [Fact]
    public void ThenExtension_WhenInvokeOnSuccessResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnSuccess(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public void ThenExtension_WhenInvokeOnFailedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new InfoError("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnSuccess(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void OnSuccessExtension_WhenInvokeOnSuccessTypedResult_ShouldBeInvoked()
    {
        // Arrange
        var value = "Hello there!";
        var result = Result.Ok(value);

        string? expected = null;
        Action<string> action = x => expected = x;

        // Act
        var thenResult = result.OnSuccess(action);

        // Assert
        thenResult.Should().Be(result);
        expected.Should().Be(value);
    }

    [Fact]
    public void OnSuccessExtension_WhenInvokeOnFailedTypedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new InfoError("Deadlock");
        var result = Result.Fail<string>(error);

        string? expected = null;
        Action<string> action = x => expected = x;

        // Act
        var thenResult = result.OnSuccess(action);

        // Assert
        thenResult.Should().Be(result);
        expected.Should().BeNull();
    }

    [Fact]
    public void OnFailExtension_WhenInvokeOnFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var error = new InfoError("Bad");
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
        var error = new InfoError("Bad");
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

    [Fact]
    public void OnFailWithExceptionExtension_WhenInvokeOnSuccessResult_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.OnFailWithException<Exception>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void OnFailWithExceptionExtension_WhenInvokeOnFailedResultWithoutExpectedException_ShouldBeNotInvoked()
    {
        // Arrange
        var exception = new Exception("Bad");
        var error = new ExceptionalError(exception);
        var result = Result.Fail(error);

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.OnFailWithException<ArgumentException>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void OnFailWithExceptionExtension_WhenInvokeOnFailedResultWithExpectedException_ShouldBeInvoked()
    {
        // Arrange
        var exception = new ArgumentException("Fault!");
        var error = new ExceptionalError(exception);
        var result = Result.Fail(error);

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.OnFailWithException<ArgumentException>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().Be(error);
    }
}