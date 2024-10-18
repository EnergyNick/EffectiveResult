using EffectiveResult.Abstractions;
using EffectiveResult.Extensions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests.Extensions;

public class ThenFailExtensionsTests
{
    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnFailedResult_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.ThenOnFail(action);

        // Assert
        thenResult.ShouldBeSuccess();
        flag.Should().BeTrue();
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnSuccessResult_ShouldNotBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Action action = () => flag = true;

        // Act
        var thenResult = result.ThenOnFail(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnFailedResultWithArgumentAction_ShouldBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.ThenOnFail(action);

        // Assert
        thenResult.ShouldBeSuccess();
        received.Should().NotBeNull().And.BeEquivalentTo(result.Errors);
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnSuccessResultWithArgumentAction_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IEnumerable<IError>? received = null;
        Action<IEnumerable<IError>> action = errors => received = errors;

        // Act
        var thenResult = result.ThenOnFail(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnSuccessResultWithFuncFactory_ShouldNotBeInvoked()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = () =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = () =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = result.ThenOnFail(action);
        var secondResult = result.ThenOnFail(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnFailedResultWithFuncFactory_ShouldBeInvokedAndReturnNewResult()
    {
        // Arrange
        var error = new Error("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = () =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = () =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = result.ThenOnFail(action);
        var secondResult = result.ThenOnFail(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnSuccessResultWithFuncByErrors_ShouldNotBeInvoked()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = (IReadOnlyCollection<IError> errors) =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = (IReadOnlyCollection<IError> errors) =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = result.ThenOnFail(action);
        var secondResult = result.ThenOnFail(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public void ThenOnFailExtension_WhenInvokeOnFailedResultWithFuncByErrors_ShouldBeInvokedAndReturnNewResult()
    {
        // Arrange
        var error = new Error("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var isInvoked = false;
        var action = (IReadOnlyCollection<IError> errors) =>
        {
            isInvoked = true;
            return internalValue;
        };

        var internalResult = Result.Ok(internalValue);
        var isInvokedResult = false;
        var actionResult = (IReadOnlyCollection<IError> errors) =>
        {
            isInvokedResult = true;
            return internalResult;
        };

        // Act
        var firstResult = result.ThenOnFail(action);
        var secondResult = result.ThenOnFail(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public void ThenOnFailWithExceptionExtension_WhenInvokeOnSuccessResult_ShouldBeNotInvoked()
    {
        // Arrange
        var result = Result.Ok();

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.ThenOnFailWithException<Exception>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void ThenOnFailWithExceptionExtension_WhenInvokeOnFailedResultWithoutExpectedException_ShouldBeNotInvoked()
    {
        // Arrange
        var exception = new Exception("Bad");
        var error = new ExceptionalError(exception);
        var result = Result.Fail(error);

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.ThenOnFailWithException<ArgumentException>(action);

        // Assert
        thenResult.Should().Be(result);
        received.Should().BeNull();
    }

    [Fact]
    public void ThenOnFailWithExceptionExtension_WhenInvokeOnFailedResultWithExpectedException_ShouldBeInvoked()
    {
        // Arrange
        var exception = new ArgumentException("Fault!");
        var error = new ExceptionalError(exception);
        var result = Result.Fail(error);

        IExceptionalError? received = null;
        Action<IExceptionalError> action = errors => received = errors;

        // Act
        var thenResult = result.ThenOnFailWithException<ArgumentException>(action);

        // Assert
        thenResult.ShouldBeSuccess();
        received.Should().Be(error);
    }
}