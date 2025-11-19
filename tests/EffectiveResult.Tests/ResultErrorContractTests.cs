namespace EffectiveResult.Tests;

public class ResultErrorContractTests
{
    [Fact]
    public void ResultConstruction_ShouldBeInValidState()
    {
        // Arrange
        const string message = "Bad";
        var exception = new Exception("Wow");

        // Act
        var error = new ResultError(message);
        var errorWithException = new ResultError(exception);
        var errorWithMessageAndException = new ResultError(message, exception);

        // Assert
        error.Message.Should().Be(message);
        error.Exception.Should().BeNull();
        error.CausedErrors.Should().BeEmpty();

        errorWithException.Message.Should().Be(exception.Message);
        errorWithException.Exception.Should().Be(exception);
        errorWithException.CausedErrors.Should().BeEmpty();

        errorWithMessageAndException.Message.Should().Be(message);
        errorWithMessageAndException.Exception.Should().Be(exception);
        errorWithMessageAndException.CausedErrors.Should().BeEmpty();
    }

    [Fact]
    public void ResultConstruction_WhenWithNested_ShouldBeInValidState()
    {
        // Arrange
        const string message = "Bad";
        var exception = new Exception("Wow");
        var nestedError = new ResultError("Other");

        // Act
        var error = new ResultError(message, nestedError);
        var errorWithException = new ResultError(exception, nestedError);
        var errorWithMessageAndException = new ResultError(message, exception, [nestedError]);

        // Assert
        error.Message.Should().Be(message);
        error.Exception.Should().BeNull();
        error.CausedErrors.Should().ContainSingle().And.AllBeEquivalentTo(nestedError);

        errorWithException.Message.Should().Be(exception.Message);
        errorWithException.Exception.Should().Be(exception);
        errorWithException.CausedErrors.Should().ContainSingle().And.AllBeEquivalentTo(nestedError);

        errorWithMessageAndException.Message.Should().Be(message);
        errorWithMessageAndException.Exception.Should().Be(exception);
        errorWithMessageAndException.CausedErrors.Should().ContainSingle().And.AllBeEquivalentTo(nestedError);
    }

    [Fact]
    public void ResultEqualsOperator_WhenCompareErrorWithMessage_ShouldReturnValidState()
    {
        // Arrange
        const string message = "Bad";

        var error1 = new ResultError(message);
        var error2 = new ResultError(message);
        var nestedError = new ResultError("Other");
        var error1WithNested = new ResultError(message, nestedError);
        var error2WithNested = new ResultError(message, nestedError);

        // Act
        var equalResult = error1.Equals(error2);
        var equalResultWithNested = error1WithNested.Equals(error2WithNested);
        var referenceEqualsResult = error1.Equals(error1);

        // Assert
        equalResult.Should().BeTrue();
        equalResultWithNested.Should().BeTrue();
        referenceEqualsResult.Should().BeTrue();
    }

    [Fact]
    public void ResultEqualsOperator_WhenCompareFailedResults_ShouldReturnValidState()
    {
        // Arrange
        var error = new ResultError("Very bad");
        var otherError = new ResultError("Very bad, but different");

        var result = Result.Fail(error);
        var resultSame = Result.Fail(error);
        var resultDifferent = Result.Fail(otherError);

        // Act
        var equalResultForSame = result.Equals(result);
        var equalResultForEquivalent = result.Equals(resultSame);
        var equalResultForDifferent = result.Equals(resultDifferent);

        // Assert
        equalResultForSame.Should().BeTrue();
        equalResultForEquivalent.Should().BeTrue();
        equalResultForDifferent.Should().BeFalse();
    }

    [Fact]
    public void ResultErrorEqualsOperator_WhenCompareWithInvalid_ShouldReturnValidState()
    {
        // Arrange
        var error = new ResultError("Test");

        // Act
        var equalResultForNull = error.Equals((object?)null);
        var equalResultForNullResult = error.Equals((ResultError?)null);

        // Assert
        equalResultForNull.Should().BeFalse();
        equalResultForNullResult.Should().BeFalse();
    }
}
