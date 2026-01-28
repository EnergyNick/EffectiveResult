using EffectiveResult.Extensions;

namespace EffectiveResult.Tests.Extensions;

public class ConclusionErrorsExtensionsTests
{
    [Fact]
    public void HasErrorOfType_WhenSearchError_ShouldReturnValidState()
    {
        // Arrange
        var error = new ResultError("Hello");
        var exceptionalError = new ResultError(new Exception("Bug"));
        var otherError = new ResultError("Salad");

        var errors = new[]
        {
            error,
            exceptionalError,
        };
        var result = Result.Fail(errors);

        // Act
        var isContainsException = result.Errors.HasErrorsOfType<ResultError>();
        var isContainsError = result.Errors.HasErrorsOfType((ResultError x) => x == error);
        var isContainsOtherError = result.Errors.HasErrorsOfType((ResultError x) => x == otherError);

        // Assert
        isContainsException.Should().BeTrue();
        isContainsError.Should().BeTrue();
        isContainsOtherError.Should().BeFalse();
    }

    [Fact]
    public void HasErrorsOfTypeRecursively_WhenSearchError_ShouldReturnValidState()
    {
        // Arrange
        var error = new ResultError("Hello");
        var internalError = new ResultError("Internal bad");
        var exceptionalError = new ResultError(new Exception("Bug"), internalError);
        var otherError = new ResultError("Salad");

        var errors = new ResultError[]
        {
            error,
            exceptionalError,
        };
        var result = Result.Fail(errors);

        // Act
        var isContainsException = result.Errors.HasErrorsOfTypeRecursively<ResultError>();
        var isContainsError = result.Errors.HasErrorsOfTypeRecursively((ResultError x) => x == error);
        var isContainsOtherError = result.Errors.HasErrorsOfTypeRecursively((ResultError x) => x == otherError);
        var isContainsInternalError = result.Errors.HasErrorsOfTypeRecursively((ResultError x) => x == internalError);

        // Assert
        isContainsException.Should().BeTrue();
        isContainsError.Should().BeTrue();
        isContainsOtherError.Should().BeFalse();
        isContainsInternalError.Should().BeTrue();
    }

    [Fact]
    public void GetErrorsOfType_WhenSearchErrors_ShouldReturnAllExceptions()
    {
        // Arrange
        var myError = new MyOtherResultError("Other type error");
        var myCausedError = new MyOtherResultError("Internal other error");

        var resultSuccess = Result.Ok();
        var failedWithoutTypedErrors = Result.Fail([new ResultError("Not my error")]);
        var failedWithTypedErrors = Result.Fail([new ResultError("Not my error"), myError]);
        var failedWithTypedErrorsAndCaused = Result.Fail(
        [
            myError,
            new ResultError(
                "Not my error",
                myCausedError)
        ]);

        // Act
        var fromSuccess = resultSuccess.GetErrorsOfType<MyOtherResultError>();
        var fromFailWithout = failedWithoutTypedErrors.GetErrorsOfType<MyOtherResultError>();
        var fromFailWithTyped = failedWithTypedErrors.GetErrorsOfType<MyOtherResultError>();
        var fromFailWithTypedNotCaused =
            failedWithTypedErrorsAndCaused.GetErrorsOfType<MyOtherResultError>();

        // Assert
        fromSuccess.Should().BeEmpty();
        fromFailWithout.Should().BeEmpty();
        fromFailWithTyped.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedNotCaused.Should().ContainSingle().And.HaveElementAt(0, myError);
    }

    [Fact]
    public void GetErrorsOfType_WhenSearchExceptionByTypeOrPredicate_ShouldReturnValidExceptions()
    {
        // Arrange
        var myError = new MyOtherResultError("Other type error");
        var myInvalidError = new MyOtherResultError("Other type error, but invalid");
        var myCausedError = new MyOtherResultError("Internal other error");

        var resultSuccess = Result.Ok();
        var failedWithoutTypedErrors = Result.Fail([myInvalidError, new ResultError("Not my error")]);
        var failedWithTypedErrors = Result.Fail([myInvalidError, new ResultError("Not my error"), myError]);
        var failedWithTypedErrorsAndCaused = Result.Fail(
        [
            myInvalidError,
            myError,
            new ResultError(
                "Not my error",
                new ResultError("Not my caused error"),
                myCausedError)
        ]);

        Predicate<MyOtherResultError> predicateByType = e => e == myError || e == myCausedError;

        // Act
        var fromSuccess = resultSuccess.GetErrorsOfType(predicateByType);
        var fromFailWithout = failedWithoutTypedErrors.GetErrorsOfType(predicateByType);
        var fromFailWithTyped = failedWithTypedErrors.GetErrorsOfType(predicateByType);
        var fromFailWithTypedNotCaused =
            failedWithTypedErrorsAndCaused.GetErrorsOfType(predicateByType);

        // Assert
        fromSuccess.Should().BeEmpty();
        fromFailWithout.Should().BeEmpty();
        fromFailWithTyped.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedNotCaused.Should().ContainSingle().And.HaveElementAt(0, myError);
    }

    [Fact]
    public void GetErrorsOfTypeRecursively_WhenSearchErrors_ShouldReturnAllExceptions()
    {
        // Arrange
        var myError = new MyOtherResultError("Other type error");
        var myCausedError = new MyOtherResultError("Internal other error");

        var resultSuccess = Result.Ok();
        var failedWithoutTypedErrors = Result.Fail([new ResultError("Not my error")]);
        var failedWithTypedErrors = Result.Fail([new ResultError("Not my error"), myError]);
        var failedWithTypedErrorsAndCaused = Result.Fail(
        [
            myError,
            new ResultError(
                "Not my error",
                new ResultError("Not my caused error"))
        ]);
        var failedWithTypedErrorsAndCausedOfNeededType = Result.Fail(
        [
            myError,
            new ResultError(
                "Not my error",
                myCausedError)
        ]);

        // Act
        var fromSuccess = resultSuccess.GetErrorsOfTypeRecursively<MyOtherResultError>();
        var fromFailWithout = failedWithoutTypedErrors.GetErrorsOfTypeRecursively<MyOtherResultError>();
        var fromFailWithTyped = failedWithTypedErrors.GetErrorsOfTypeRecursively<MyOtherResultError>();
        var fromFailWithTypedNotCaused =
            failedWithTypedErrorsAndCaused.GetErrorsOfTypeRecursively<MyOtherResultError>();
        var fromFailWithTypedAndCaused =
            failedWithTypedErrorsAndCausedOfNeededType.GetErrorsOfTypeRecursively<MyOtherResultError>();

        // Assert
        fromSuccess.Should().BeEmpty();
        fromFailWithout.Should().BeEmpty();
        fromFailWithTyped.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedNotCaused.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedAndCaused.Should().HaveCount(2)
            .And.AllBeOfType<MyOtherResultError>()
            .And.IntersectWith([myError, myCausedError]);
    }

    [Fact]
    public void GetErrorsOfTypeRecursively_WhenSearchExceptionByTypeOrPredicate_ShouldReturnValidExceptions()
    {
        // Arrange
        var myError = new MyOtherResultError("Other type error");
        var myInvalidError = new MyOtherResultError("Other type error, but invalid");
        var myCausedError = new MyOtherResultError("Internal other error");
        var myCausedInvalidError = new MyOtherResultError("Internal other error, but invalid");

        var resultSuccess = Result.Ok();
        var failedWithoutTypedErrors = Result.Fail([myInvalidError, new ResultError("Not my error")]);
        var failedWithTypedErrors = Result.Fail([myInvalidError, new ResultError("Not my error"), myError]);
        var failedWithTypedErrorsAndCaused = Result.Fail(
        [
            myInvalidError,
            myError,
            new ResultError(
                "Not my error",
                new ResultError("Not my caused error"),
                myCausedInvalidError)
        ]);
        var failedWithTypedErrorsAndCausedOfNeededType = Result.Fail(
        [
            myError,
            new ResultError(
                "Not my error",
                myCausedError,
                myCausedInvalidError)
        ]);

        Predicate<MyOtherResultError> predicateByType = e => e == myError || e == myCausedError;

        // Act
        var fromSuccess = resultSuccess.GetErrorsOfTypeRecursively(predicateByType);
        var fromFailWithout = failedWithoutTypedErrors.GetErrorsOfTypeRecursively(predicateByType);
        var fromFailWithTyped = failedWithTypedErrors.GetErrorsOfTypeRecursively(predicateByType);
        var fromFailWithTypedNotCaused =
            failedWithTypedErrorsAndCaused.GetErrorsOfTypeRecursively(predicateByType);
        var fromFailWithTypedAndCaused =
            failedWithTypedErrorsAndCausedOfNeededType.GetErrorsOfTypeRecursively(predicateByType);

        // Assert
        fromSuccess.Should().BeEmpty();
        fromFailWithout.Should().BeEmpty();
        fromFailWithTyped.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedNotCaused.Should().ContainSingle().And.HaveElementAt(0, myError);
        fromFailWithTypedAndCaused.Should().HaveCount(2)
            .And.AllBeOfType<MyOtherResultError>()
            .And.IntersectWith([myError, myCausedError]);
    }

    [Fact]
    public void GetExceptions_WhenSearchExceptions_ShouldReturnAllExceptions()
    {
        // Arrange
        var error = new ResultError("Hello");
        var internalError = new ResultError("Internal bad");
        var exceptionalError = new ResultError(new Exception("Bug"), internalError);
        var otherError = new ResultError("Salad");

        var resultSuccess = Result.Ok();
        var resultFail = Result.Fail([error, otherError]);
        var resultFailWithExceptions = Result.Fail([error, exceptionalError]);

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

        var error = new ResultError("Hello");
        var internalError = new ResultError("Internal bad");

        var exceptionalErrorInvalid = new ResultError(invalidOperationException, internalError);
        var exceptionalErrorOutOfRange = new ResultError(indexOutOfRangeException, internalError);

        var resultFailWithExceptions =
            Result.Fail([error, exceptionalErrorInvalid, exceptionalErrorOutOfRange]);

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
        var error = new ResultError("Hello");
        var internalError = new ResultError("Internal bad");
        var exceptionalError = new ResultError(new Exception("Bug"), internalError);
        var otherError = new ResultError("Salad");

        var resultSuccess = Result.Ok();
        var resultFail = Result.Fail([error, otherError]);
        var resultFailWithExceptions = Result.Fail([error, exceptionalError]);

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

        var error = new ResultError("Hello");
        var internalError = new ResultError("Internal bad");

        var exceptionalErrorInvalid = new ResultError(invalidOperationException, internalError);
        var exceptionalErrorOutOfRange = new ResultError(indexOutOfRangeException, internalError);

        var resultFailWithExceptions =
            Result.Fail([error, exceptionalErrorInvalid, exceptionalErrorOutOfRange]);

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

    private record MyOtherResultError : ResultError
    {
        public MyOtherResultError(string message, params ResultError[] causedErrors)
            : base(message, causedErrors)
        {
        }
    }
}
