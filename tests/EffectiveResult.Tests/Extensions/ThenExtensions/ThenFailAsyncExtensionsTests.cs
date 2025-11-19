using EffectiveResult.Extensions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests.Extensions.ThenExtensions;

public class ThenFailAsyncExtensionsTests
{
    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var error = new ResultError("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Func<Task> action = async () => flag = true;

        // Act
        var thenResult = await result.ThenOnFailAsync(action);

        // Assert
        thenResult.ShouldBeSuccess();
        flag.Should().BeTrue();
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Func<Task> action = async () => flag = true;

        // Act
        var thenResult = await result.ThenOnFailAsync(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnFailedResultWithArgumentAction_ShouldBeInvoked()
    {
        // Arrange
        var error = new ResultError("Bad");
        var result = Result.Fail(error);

        IEnumerable<ResultError>? received = null;
        Func<IEnumerable<ResultError>, Task> action = async errors => received = errors;

        // Act
        var thenResult = await result.ThenOnFailAsync(action);

        // Assert
        thenResult.ShouldBeSuccess();
        received.Should().NotBeNull().And.BeEquivalentTo(result.Errors);
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnSuccessResultWithArgumentAction_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IEnumerable<ResultError>? received = null;
        Func<IEnumerable<ResultError>, Task> action = async errors => received = errors;

        // Act
        var thenResult = await result.ThenOnFailAsync(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnSuccessResultWithFuncFactory_ShouldNotBeInvoked()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = async () =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = async () =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = await result.ThenOnFailAsync(action);
        var secondResult = await result.ThenOnFailAsync(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnFailedResultWithFuncFactory_ShouldBeInvokedAndReturnNewResult()
    {
        // Arrange
        var error = new ResultError("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = async () =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = async () =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = await result.ThenOnFailAsync(action);
        var secondResult = await result.ThenOnFailAsync(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnSuccessResultWithFuncByErrors_ShouldNotBeInvoked()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = async (IReadOnlyCollection<ResultError> errors) =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = async (IReadOnlyCollection<ResultError> errors) =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = await result.ThenOnFailAsync(action);
        var secondResult = await result.ThenOnFailAsync(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public async Task ThenOnFailAsyncExtension_WhenInvokeOnFailedResultWithFuncByErrors_ShouldBeInvokedAndReturnNewResult()
    {
        // Arrange
        var error = new ResultError("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = async (IReadOnlyCollection<ResultError> errors) =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = async (IReadOnlyCollection<ResultError> errors) =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = await result.ThenOnFailAsync(action);
        var secondResult = await result.ThenOnFailAsync(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public async Task ThenOnFailWithExceptionExtension_WhenInvokeOnSuccessResult_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        ResultError? received = null;
        Func<ResultError, Task> action = async errors => received = errors;

        // Act
        var thenResult = await result.ThenOnFailWithExceptionAsync<Exception>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public async Task ThenOnFailWithExceptionExtension_WhenInvokeOnFailedResultWithoutExpectedException_ShouldBeNotInvoked()
    {
        // Arrange
        var exception = new Exception("Bad");
        var error = new ResultError(exception);
        var result = Result.Fail(error);

        ResultError? received = null;
        Func<ResultError, Task> action = async errors => received = errors;

        // Act
        var thenResult = await result.ThenOnFailWithExceptionAsync<ArgumentException>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public async Task ThenOnFailWithExceptionExtension_WhenInvokeOnFailedResultWithExpectedException_ShouldBeInvoked()
    {
        // Arrange
        var exception = new ArgumentException("Fault!");
        var error = new ResultError(exception);
        var result = Result.Fail(error);

        ResultError? received = null;
        Func<ResultError, Task> action = async errors => received = errors;

        // Act
        var thenResult = await result.ThenOnFailWithExceptionAsync<ArgumentException>(action);

        // Assert
        thenResult.ShouldBeSuccess();
        received.Should().Be(error);
    }
}
