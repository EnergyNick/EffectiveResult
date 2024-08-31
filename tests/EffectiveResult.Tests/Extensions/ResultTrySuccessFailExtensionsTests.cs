using EffectiveResult.Abstractions;
using EffectiveResult.Extensions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests.Extensions;

public class ResultTrySuccessFailExtensionsTests
{
    [Fact]
    public void OnSuccessTryExtension_WhenInvokeOnSuccessResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnSuccessTry(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public void OnSuccessTryExtension_WhenInvokeOnFailedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnSuccessTry(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void OnSuccessTryExtension_WhenInvokeOnSuccessTypedResult_ShouldBeInvoked()
    {
        // Arrange
        var value = "Hello there!";
        var result = Result.Ok(value);

        string? expected = null;
        Action<string> action = x => expected = x;

        // Act
        var thenResult = result.OnSuccessTry(action);

        // Assert
        thenResult.Should().Be(result);
        expected.Should().Be(value);
    }

    [Fact]
    public void OnSuccessTryExtension_WhenInvokeOnFailedTypedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Deadlock");
        var result = Result.Fail<string>(error);

        string? expected = null;
        Action<string> action = x => expected = x;

        // Act
        var thenResult = result.OnSuccessTry(action);

        // Assert
        thenResult.Should().Be(result);
        expected.Should().BeNull();
    }

    [Fact]
    public void OnSuccessTryExtension_WhenInvokeWithExceptionOnSuccessResult_ShouldReturnFailedResult()
    {
        // Arrange
        var result = Result.Ok();
        var valuedResult = Result.Ok(5);

        var exception = new Exception("Bad situation");
        var expectedError = new ExceptionalError(exception);

        Action action = () => throw exception;
        Action<int> valuedAction = _ => throw exception;

        // Act
        var resultAction = () => result.OnSuccessTry(action);
        var valuedResultAction = () => valuedResult.OnSuccessTry(valuedAction);

        // Assert
        resultAction.Should().NotThrow();
        resultAction().ShouldBeFailed(expectedError);

        valuedResultAction.Should().NotThrow();
        valuedResultAction().ShouldBeFailed(expectedError);
    }

    [Fact]
    public void OnFailTryExtension_WhenInvokeOnFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnFailTry(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public void OnFailTryExtension_WhenInvokeOnSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.OnFailTry(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void OnFailTryExtension_WhenInvokeOnFailedResultWithArgumentAction_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.OnFailTry(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().NotBeNull().And.BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void OnFailTryExtension_WhenInvokeOnSuccessResultWithArgumentAction_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.OnFailTry(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void OnFailTryExtension_WhenInvokeWithExceptionOnFailedResult_ShouldReturnFailedResultWithNewError()
    {
        // Arrange
        var initialError = new Error("Very bad");
        var result = Result.Fail(initialError);
        var valuedResult = Result.Fail<int>(initialError);

        var exception = new Exception("Bad situation");
        var expectedError = new ExceptionalError(exception);

        Action action = () => throw exception;
        Action<IEnumerable<IError>> errorsAction = _ => throw exception;

        // Act
        var resultFirstAction = () => result.OnFailTry(action);
        var resultSecondAction = () => result.OnFailTry(errorsAction);
        var valuedResultFirstAction = () => valuedResult.OnFailTry(action);
        var valuedResultSecondAction = () => valuedResult.OnFailTry(errorsAction);

        // Assert
        resultFirstAction.Should().NotThrow();
        resultSecondAction.Should().NotThrow();
        resultFirstAction().ShouldBeFailed(initialError, expectedError);
        resultSecondAction().ShouldBeFailed(initialError, expectedError);

        valuedResultFirstAction.Should().NotThrow();
        valuedResultSecondAction.Should().NotThrow();
        valuedResultFirstAction().ShouldBeFailed(initialError, expectedError);
        valuedResultSecondAction().ShouldBeFailed(initialError, expectedError);
    }
}