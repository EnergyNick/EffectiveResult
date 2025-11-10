using EffectiveResult.Extensions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests.Extensions.ThenExtensions;

public class ThenAsyncExtensionsTests
{
    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();
        var flag = false;
        Func<Task> action = async () => flag = true;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeTrue();
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Bad");
        var result = Result.Fail(error);

        var flag = false;
        Func<Task> action = async () => flag = true;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.Should().Be(result);
        flag.Should().BeFalse();
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessTypedResult_ShouldBeInvoked()
    {
        // Arrange
        var value = "Hello there!";
        var result = Result.Ok(value);

        string? expected = null;
        Func<string, Task> action = async x => expected = x;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeSuccess();
        expected.Should().Be(value);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedTypedResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Deadlock");
        var result = Result.Fail<string>(error);

        string? expected = null;
        Func<string, Task> action = async x => expected = x;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeFailed();
        expected.Should().BeNull();
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessResultAndFunc_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var result = Result.Ok();

        var value = "Hello there!";
        var action = async () => value;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeSuccessAndReferenceEqualsValue(value);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedResultAndFunc_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Deadlock");
        var result = Result.Fail(error);

        var value = "Hello there!";
        var action = async () => value;

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeFailed(error);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessResultWithFuncReturningResult_ShouldBeInvoked()
    {
        // Arrange
        var result = Result.Ok();

        var internalError = new ResultError("Situation is terrible!");
        var internalSuccessResult = Result.Ok();
        var internalFailedResult = Result.Fail(internalError);

        var successAction = async () => internalSuccessResult;
        var failedAction = async () => internalFailedResult;

        // Act
        var thenSuccessResult = await result.ThenAsync(successAction);
        var thenFailedResult = await result.ThenAsync(failedAction);

        // Assert
        thenSuccessResult.Should().Be(internalSuccessResult);
        thenFailedResult.Should().Be(internalFailedResult);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedResultWithFuncReturningResult_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Bad");
        var result = Result.Fail(error);

        var internalError = new ResultError("Situation is terrible!");
        var internalSuccessResult = Result.Ok();
        var internalFailedResult = Result.Fail(internalError);

        var successAction = async () => internalSuccessResult;
        var failedAction = async () => internalFailedResult;

        // Act
        var thenSuccessResult = await result.ThenAsync(successAction);
        var thenFailedResult = await result.ThenAsync(failedAction);

        // Assert
        thenSuccessResult.Should().Be(result);
        thenFailedResult.Should().Be(result);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var result = Result.Ok();

        var value = "Hello there!";
        var error = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok(value);
        var expectedFailedResult = Result.Fail<string>(error);
        var successAction = async () => expectedSuccessResult;
        var failedAction = async () => expectedFailedResult;

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccessAndReferenceEqualsValue(value);
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedResultAndFuncWithResultReturn_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Deadlock");
        var result = Result.Fail(error);

        var value = "Hello there!";
        var internalError = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok(value);
        var expectedFailedResult = Result.Fail(internalError);
        var successAction = async () => expectedSuccessResult;
        var failedAction = async () => expectedFailedResult;

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessTypedResultAndFunc_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";

        string? expectedValues = null;
        var action = async (string x) =>
        {
            expectedValues = x;
            return internalValue;
        };

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeSuccessAndEqualsValue(internalValue);
        expectedValues.Should().Be(value);
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedTypedResultAndFunc_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Deadlocker");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";

        string? expectedValues = null;
        var action = async (string x) =>
        {
            expectedValues = x;
            return internalValue;
        };

        // Act
        var thenResult = await result.ThenAsync(action);

        // Assert
        thenResult.ShouldBeFailed(error);
        expectedValues.Should().BeNull();

    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessTypedResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var error = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok();
        var expectedFailedResult = Result.Fail(error);

        var expectedValues = new List<string>();
        var successAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccess();
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);

        expectedValues.Should().OnlyContain(x => ReferenceEquals(x, value));
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedTypedResultAndFuncWithResultReturn_ShouldNotBeInvokedAndReturnResult()
    {
        // Arrange
        var error = new ResultError("Deadlock");
        var result = Result.Fail<string>(error);

        var internalError = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok();
        var expectedFailedResult = Result.Fail(internalError);

        var expectedValues = new List<string>();
        var successAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);

        expectedValues.Should().BeEmpty();
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnSuccessTypedResultAndFuncWithResultReturn_ShouldBeInvokedAndReturnTypedResult()
    {
        // Arrange
        var value = "Good time need good result";
        var result = Result.Ok(value);

        var internalValue = "Hello there!";
        var error = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok(internalValue);
        var expectedFailedResult = Result.Fail<string>(error);

        var expectedValues = new List<string>();
        var successAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeSuccessAndReferenceEqualsValue(internalValue);
        thenResultSuccess.Should().Be(expectedSuccessResult);

        thenResultFailed.ShouldBeFailed(error);
        thenResultFailed.Should().Be(expectedFailedResult);

        expectedValues.Should().OnlyContain(x => ReferenceEquals(x, value));
    }

    [Fact]
    public async Task ThenAsyncExtension_WhenInvokeOnFailedTypedResultAndFuncWithResultReturn_ShouldNotBeInvoked()
    {
        // Arrange
        var error = new ResultError("Deadlock");
        var result = Result.Fail<string>(error);

        var internalValue = "Hello there!";
        var internalError = new ResultError("Not functional?!");

        var expectedSuccessResult = Result.Ok(internalValue);
        var expectedFailedResult = Result.Fail<string>(internalError);

        var expectedValues = new List<string>();
        var successAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedSuccessResult;
        };
        var failedAction = async (string x) =>
        {
            expectedValues.Add(x);
            return expectedFailedResult;
        };

        // Act
        var thenResultSuccess = await result.ThenAsync(successAction);
        var thenResultFailed = await result.ThenAsync(failedAction);

        // Assert
        thenResultSuccess.ShouldBeFailed(error);
        thenResultFailed.ShouldBeFailed(error);

        expectedValues.Should().BeEmpty();
    }
}
