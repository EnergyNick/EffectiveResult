using EffectiveResult.Extensions;

namespace EffectiveResult.Tests.Extensions;

public class MatchExtensionTests
{
    [Fact]
    public void Match_WhenResultIsSuccess_ShouldCallSuccessMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Ok();

        // Act
        result.Match(() => successCallCount++, () => failedCallCount++);
        result.Match(() => successCallCount++, _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(2);
        failedCallCount.Should().Be(0);
        failedWithArgsCallCount.Should().Be(0);
    }

    [Fact]
    public void Match_WhenResultIsSuccessWithValue_ShouldCallSuccessMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Ok(123);

        // Act
        result.Match(() => successCallCount++, () => failedCallCount++);
        result.Match((int _) => successCallCount++, () => failedCallCount++);
        result.Match(() => successCallCount++, _ => failedWithArgsCallCount++);
        result.Match((int _) => successCallCount++, _ => failedCallCount++);

        // Assert
        successCallCount.Should().Be(4);
        failedCallCount.Should().Be(0);
        failedWithArgsCallCount.Should().Be(0);
    }

    [Fact]
    public void Match_WhenResultIsFailed_ShouldCallFailedMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Fail("Bad");

        // Act
        result.Match(() => successCallCount++, () => failedCallCount++);
        result.Match(() => successCallCount++, _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(0);
        failedCallCount.Should().Be(1);
        failedWithArgsCallCount.Should().Be(1);
    }

    [Fact]
    public void Match_WhenResultIsFailedWithValue_ShouldCallFailedMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Fail<int>("Bad");

        // Act
        result.Match(() => successCallCount++, () => failedCallCount++);
        result.Match((int _) => successCallCount++, () => failedCallCount++);
        result.Match(() => successCallCount++, _ => failedWithArgsCallCount++);
        result.Match((int _) => successCallCount++, _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(0);
        failedCallCount.Should().Be(2);
        failedWithArgsCallCount.Should().Be(2);
    }

    [Fact]
    public async Task MatchAsync_WhenResultIsSuccess_ShouldCallSuccessMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Ok();

        // Act
        await result.MatchAsync(async () => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async () => successCallCount++, async _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(2);
        failedCallCount.Should().Be(0);
        failedWithArgsCallCount.Should().Be(0);
    }

    [Fact]
    public async Task MatchAsync_WhenResultIsSuccessWithValue_ShouldCallSuccessMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Ok(123);

        // Act
        await result.MatchAsync(async () => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async (int _) => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async () => successCallCount++, async _ => failedWithArgsCallCount++);
        await result.MatchAsync(async (int _) => successCallCount++, async _ => failedCallCount++);

        // Assert
        successCallCount.Should().Be(4);
        failedCallCount.Should().Be(0);
        failedWithArgsCallCount.Should().Be(0);
    }

    [Fact]
    public async Task MatchAsync_WhenResultIsFailed_ShouldCallFailedMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Fail("Bad");

        // Act
        await result.MatchAsync(async () => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async () => successCallCount++, async _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(0);
        failedCallCount.Should().Be(1);
        failedWithArgsCallCount.Should().Be(1);
    }

    [Fact]
    public async Task MatchAsync_WhenResultIsFailedWithValue_ShouldCallFailedMethod()
    {
        // Arrange
        var successCallCount = 0;
        var failedCallCount = 0;
        var failedWithArgsCallCount = 0;

        var result = Result.Fail<int>("Bad");

        // Act
        await result.MatchAsync(async () => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async (int _) => successCallCount++, async () => failedCallCount++);
        await result.MatchAsync(async () => successCallCount++, async _ => failedWithArgsCallCount++);
        await result.MatchAsync(async (int _) => successCallCount++, async _ => failedWithArgsCallCount++);

        // Assert
        successCallCount.Should().Be(0);
        failedCallCount.Should().Be(2);
        failedWithArgsCallCount.Should().Be(2);
    }
}
