using FunctionalResult.Abstractions;
using FunctionalResult.Extensions;
using FunctionalResult.TestsCommon.Helpers;

namespace FunctionalResult.Tests.Extensions;

public class ThenTryExtensionsTests
{
    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessTypedResult_ShouldBeInvoked()
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
    public void ThenTryExtension_WhenThrowOnSuccessTypedResult_ShouldReturnFailedResult()
    {
        // Arrange
        var value = "Hello there!";
        var result = Result.Ok(value);

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Action<string> action = _ => throw exception;

        // Act
        var thenResultAction = () => result.OnSuccessTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedTypedResult_ShouldNotBeInvoked()
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
    public void ThenTryExtension_WhenInvokeOnSuccessResultAndFunc_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var result = Result.Ok();

        var value = "Hello there!";
        var action = () => value;

        // Act
        var thenResult = result.ThenTry(action);

        // Assert
        thenResult.ShouldBeSuccessAndReferenceEqualsValue(value);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessResultAndFuncWithThrow_ShouldReturnFailedResult()
    {
        // Arrange
        var result = Result.Ok();

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<string> action = () => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedResultAndFunc_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Deadlock");
        var result = Result.Fail(error);

        var value = "Hello there!";
        var action = () => value;

        // Act
        var thenResult = result.ThenTry(action);

        // Assert
        thenResult.ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessResultWithFuncReturningResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();

        var internalError = new Error("Situation is terrible!");
        var internalSuccessResult = Result.Ok();
        var internalFailedResult = Result.Fail(internalError);

        var successAction = () => internalSuccessResult;
        var failedAction = () => internalFailedResult;

        // Act
        var thenSuccessResult = result.ThenTry(successAction);
        var thenFailedResult = result.ThenTry(failedAction);

        // Assert
        thenSuccessResult.Should().Be(internalSuccessResult);
        thenFailedResult.Should().Be(internalFailedResult);
    }

    [Fact]
    public void ThenTryExtension_WhenThrowOnSuccessResultWithFuncReturningResult_ShouldReturnFailedResult()
    {
        // Arrange
        var result = Result.Ok();

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<Result> action = () => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedResultWithFuncReturningResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Bad");
        var result = Result.Fail(error);

        var internalError = new Error("Situation is terrible!");
        var internalSuccessResult = Result.Ok();
        var internalFailedResult = Result.Fail(internalError);

        var successAction = () => internalSuccessResult;
        var failedAction = () => internalFailedResult;

        // Act
        var thenSuccessResult = result.ThenTry(successAction);
        var thenFailedResult = result.ThenTry(failedAction);

        // Assert
        thenSuccessResult.Should().Be(result);
        thenFailedResult.Should().Be(result);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var result = Result.Ok();

        var value = "Hello there!";
        var error = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok(value);
        var expectedFailedResult = Result.Fail<string>(error);
        var successAction = () => expectedSuccessResult;
        var failedAction = () => expectedFailedResult;

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccessAndReferenceEqualsValue(value);
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);
    }

    [Fact]
    public void ThenTryExtension_WhenThrowOnSuccessResultAndFuncWithTypedResultReturn_ShouldReturnFailedResult()
    {
        // Arrange
        var result = Result.Ok();

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<Result<string>> action = () => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedResultAndFuncWithResultReturn_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Deadlock");
        var result = Result.Fail(error);

        var value = "Hello there!";
        var internalError = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok(value);
        var expectedFailedResult = Result.Fail(internalError);
        var successAction = () => expectedSuccessResult;
        var failedAction = () => expectedFailedResult;

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessTypedResultAndFunc_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";

        string? expectedValues = null;
        var action = (string x) =>
        {
            expectedValues = x;
            return internalValue;
        };

        // Act
        var thenResult = result.ThenTry(action);

        // Assert
        thenResult.ShouldBeSuccessAndEqualsValue(internalValue);
        expectedValues.Should().Be(value);
    }

    [Fact]
    public void ThenTryExtension_WhenThrowOnSuccessTypedResultAndFunc_ShouldReturnFailedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<string, string> action = _ => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedTypedResultAndFunc_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";

        string? expectedValues = null;
        var action = (string x) =>
        {
            expectedValues = x;
            return internalValue;
        };

        // Act
        var thenResult = result.ThenTry(action);

        // Assert
        thenResult.ShouldBeFailed(error);
        expectedValues.Should().BeNull();

    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessTypedResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var error = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok();
        var expectedFailedResult = Result.Fail(error);

        var expectedValues = new List<string>();
        var successAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccess();
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);

        expectedValues.Should().OnlyContain(x => ReferenceEquals(x, value));
    }

    [Fact]
    public void ThenTryExtension_WhenThrowOnSuccessResultAndFuncWithResultReturn_ShouldReturnFailedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<string, Result> action = _ => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedTypedResultAndFuncWithResultReturn_ShouldNotBeInvokedAndReturnResult()
    {
        // Arrange
        var error = new Error("Deadlock");
        var result = Result.Fail<string>(error);

        var internalError = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok();
        var expectedFailedResult = Result.Fail(internalError);

        var expectedValues = new List<string>();
        var successAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);

        expectedValues.Should().BeEmpty();
    }

    [Fact]
    public void ThenTryExtension_WhenInvokeOnSuccessTypedResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var error = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok(internalValue);
        var expectedFailedResult = Result.Fail<string>(error);

        var expectedValues = new List<string>();
        var successAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);

        expectedValues.Should().OnlyContain(x => ReferenceEquals(x, value));
    }

    [Fact]
    public void ThenTryExtension_WhenThrowOnSuccessTypedResultAndFuncWithResultReturn_ShouldReturnFailedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var exception = new Exception("So bad situation");
        var error = new ExceptionalError(exception);
        Func<string, Result<string>> action = _ => throw exception;

        // Act
        var thenResultAction = () => result.ThenTry(action);

        // Assert
        thenResultAction.Should().NotThrow();
        thenResultAction().ShouldBeFailed(error);
    }


    [Fact]
    public void ThenTryExtension_WhenInvokeOnFailedTypedResultAndFuncWithResultReturn_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new Error("Deadlock");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var internalError = new Error("Not functional?!");

        var expectedSuccessResult = Result.Ok(internalValue);
        var expectedFailedResult = Result.Fail<string>(internalError);

        var expectedValues = new List<string>();
        var successAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = result.ThenTry(successAction);
        var thenResultFailed = result.ThenTry(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);

        expectedValues.Should().BeEmpty();
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenInvokeOnSuccessResultWithFuncFactory_ShouldNotBeInvoked()
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
        var firstResult = result.ThenTryOnFail(action);
        var secondResult = result.ThenTryOnFail(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenInvokeOnFailedResultWithFuncFactory_ShouldBeInvokedAndReturnNewResult()
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
        var firstResult = result.ThenTryOnFail(action);
        var secondResult = result.ThenTryOnFail(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenInvokeOnSuccessResultWithFuncByErrors_ShouldNotBeInvoked()
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
        var firstResult = result.ThenTryOnFail(action);
        var secondResult = result.ThenTryOnFail(actionResult);

        // Assert
        firstResult.Should().Be(result);
        secondResult.Should().Be(result);
        isInvoked.Should().BeFalse();
        isInvokedResult.Should().BeFalse();
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenInvokeOnFailedResultWithFuncByErrors_ShouldBeInvokedAndReturnNewResult()
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
        var firstResult = result.ThenTryOnFail(action);
        var secondResult = result.ThenTryOnFail(actionResult);

        // Assert
        firstResult.Should().NotBe(result);
        secondResult.Should().NotBe(result);
        isInvoked.Should().BeTrue();
        isInvokedResult.Should().BeTrue();

        firstResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        secondResult.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenThrowOnFailedResultAndFuncByErrors_ShouldReturnFailedResult()
    {
        // Arrange
        var error = new Error("Deadlocker");
        var result = Result.Fail<string>(error);

        var exception = new Exception("So bad situation");
        var expectedError = new ExceptionalError(exception);
        Func<IReadOnlyCollection<IError>, string> action = _ => throw exception;
        Func<IReadOnlyCollection<IError>, Result<string>> resultAction = _ => throw exception;

        // Act
        var firstThenResultAction = () => result.ThenTryOnFail(action);
        var secondThenResultAction = () => result.ThenTryOnFail(resultAction);

        // Assert
        firstThenResultAction.Should().NotThrow();
        firstThenResultAction().ShouldBeFailed(error, expectedError);

        secondThenResultAction.Should().NotThrow();
        secondThenResultAction().ShouldBeFailed(error, expectedError);
    }

    [Fact]
    public void ThenTryOnFailExtension_WhenThrowOnFailedResultAndFuncFactory_ShouldReturnFailedResult()
    {
        // Arrange
        var error = new Error("Deadlocker");
        var result = Result.Fail<string>(error);

        var exception = new Exception("So bad situation");
        var expectedError = new ExceptionalError(exception);
        Func<string> action = () => throw exception;
        Func<Result<string>> resultAction = () => throw exception;

        // Act
        var firstThenResultAction = () => result.ThenTryOnFail(action);
        var secondThenResultAction = () => result.ThenTryOnFail(resultAction);

        // Assert
        firstThenResultAction.Should().NotThrow();
        firstThenResultAction().ShouldBeFailed(error, expectedError);

        secondThenResultAction.Should().NotThrow();
        secondThenResultAction().ShouldBeFailed(error, expectedError);
    }
}