using SimpleResult.Core;
using SimpleResult.Extensions;

namespace SimpleResult.Tests.Extensions;

public class ConclusionErrorsExtensionsTests
{
    [Fact]
    public void HasErrorOfType_WhenSearchError_ShouldReturnValidState()
    {
        // Arrange
        var error = new Error("Hello");
        var exceptionalError = new ExceptionalError(new Exception("Bug"));
        var otherError = new Error("Salad");

        var errors = new IError[]
        {
            error,
            exceptionalError,
        };
        var result = Result.Fail(errors);

        // Act
        var isContainsException = result.Errors.HasErrorsOfType<ExceptionalError>();
        var isContainsError = result.Errors.HasErrorsOfType((Error x) => x == error);
        var isContainsOtherError = result.Errors.HasErrorsOfType((Error x) => x == otherError);

        // Assert
        isContainsException.Should().BeTrue();
        isContainsError.Should().BeTrue();
        isContainsOtherError.Should().BeFalse();
    }

    [Fact]
    public void HasErrorsOfTypeRecursively_WhenSearchError_ShouldReturnValidState()
    {
        // Arrange
        var error = new Error("Hello");
        var internalError = new Error("Internal bad");
        var exceptionalError = new ExceptionalError(new Exception("Bug"), internalError);
        var otherError = new Error("Salad");

        var errors = new IError[]
        {
            error,
            exceptionalError,
        };
        var result = Result.Fail(errors);

        // Act
        var isContainsException = result.Errors.HasErrorsOfTypeRecursively<ExceptionalError>();
        var isContainsError = result.Errors.HasErrorsOfTypeRecursively((Error x) => x == error);
        var isContainsOtherError = result.Errors.HasErrorsOfTypeRecursively((Error x) => x == otherError);
        var isContainsInternalError = result.Errors.HasErrorsOfTypeRecursively((Error x) => x == internalError);

        // Assert
        isContainsException.Should().BeTrue();
        isContainsError.Should().BeTrue();
        isContainsOtherError.Should().BeFalse();
        isContainsInternalError.Should().BeTrue();
    }

    [Fact]
    public void GetExceptions_WhenSearchExceptions_ShouldReturnAllExceptions()
    {
        // Arrange
        var error = new Error("Hello");
        var internalError = new Error("Internal bad");
        var exceptionalError = new ExceptionalError(new Exception("Bug"), internalError);
        var otherError = new Error("Salad");

        var resultSuccess = Result.Ok();
        var resultFail = Result.Fail(new IError[] { error, otherError });
        var resultFailWithExceptions = Result.Fail(new IError[] { error, exceptionalError });

        // Act
        var fromSuccess = resultSuccess.GetExceptions();
        var fromFail = resultFail.GetExceptions();
        var fromFailWithExceptions = resultFailWithExceptions.GetExceptions();

        // Assert
        fromSuccess.Should().BeEmpty();
        fromFail.Should().BeEmpty();
        fromFailWithExceptions.Should().ContainSingle().And.HaveElementAt(0, exceptionalError.Exception);
    }

    [Fact]
    public void GetExceptions_WhenSearchExceptionByTypeOrPredicate_ShouldReturnValidExceptions()
    {
        // Arrange
        var invalidOperationException = new InvalidOperationException("100 + 5 = 30?");
        var indexOutOfRangeException = new IndexOutOfRangeException();

        var error = new Error("Hello");
        var internalError = new Error("Internal bad");

        var exceptionalErrorInvalid = new ExceptionalError(invalidOperationException, internalError);
        var exceptionalErrorOutOfRange = new ExceptionalError(indexOutOfRangeException, internalError);

        var resultFailWithExceptions =
            Result.Fail(new IError[] { error, exceptionalErrorInvalid, exceptionalErrorOutOfRange });

        Predicate<Exception> predicate = x => x == invalidOperationException;
        Predicate<IndexOutOfRangeException> predicateByType = _ => true;

        // Act
        var fromPredicate = resultFailWithExceptions.GetExceptions(predicate);
        var fromTypedPredicate = resultFailWithExceptions.GetExceptions(predicateByType);

        // Assert
        fromPredicate.Should().ContainSingle().And.HaveElementAt(0, invalidOperationException);
        fromTypedPredicate.Should().ContainSingle().And.HaveElementAt(0, indexOutOfRangeException);
    }

    [Fact]
    public void TryGetException_WhenSearchExceptions_ShouldReturnAllExceptions()
    {
        // Arrange
        var error = new Error("Hello");
        var internalError = new Error("Internal bad");
        var exceptionalError = new ExceptionalError(new Exception("Bug"), internalError);
        var otherError = new Error("Salad");

        var resultSuccess = Result.Ok();
        var resultFail = Result.Fail(new IError[] { error, otherError });
        var resultFailWithExceptions = Result.Fail(new IError[] { error, exceptionalError });

        // Act
        var fromSuccessState = resultSuccess.TryGetException(out var fromSuccess);
        var fromFailState = resultFail.TryGetException(out var fromFail);
        var fromFailWithExceptionsState = resultFailWithExceptions.TryGetException(out var fromFailWithExceptions);

        // Assert
        fromSuccessState.Should().BeFalse();
        fromFailState.Should().BeFalse();
        fromFailWithExceptionsState.Should().BeTrue();

        fromSuccess.Should().BeNull();
        fromFail.Should().BeNull();
        fromFailWithExceptions.Should().Be(exceptionalError.Exception);
    }

    [Fact]
    public void TryGetException_WhenSearchExceptionByTypeOrPredicate_ShouldReturnValidExceptions()
    {
        // Arrange
        var invalidOperationException = new InvalidOperationException("100 + 5 = 30?");
        var indexOutOfRangeException = new IndexOutOfRangeException();

        var error = new Error("Hello");
        var internalError = new Error("Internal bad");

        var exceptionalErrorInvalid = new ExceptionalError(invalidOperationException, internalError);
        var exceptionalErrorOutOfRange = new ExceptionalError(indexOutOfRangeException, internalError);

        var resultFailWithExceptions =
            Result.Fail(new IError[] { error, exceptionalErrorInvalid, exceptionalErrorOutOfRange });

        Predicate<Exception> predicate = x => x == invalidOperationException;
        Predicate<IndexOutOfRangeException> predicateByType = _ => true;

        // Act
        var fromPredicateState = resultFailWithExceptions.TryGetException(out var fromPredicate, predicate);
        var fromTypedPredicateState = resultFailWithExceptions.TryGetException(out var fromTypedPredicate, predicateByType);

        // Assert
        fromPredicateState.Should().BeTrue();
        fromTypedPredicateState.Should().BeTrue();

        fromPredicate.Should().Be(invalidOperationException);
        fromTypedPredicate.Should().Be(indexOutOfRangeException);
    }
}