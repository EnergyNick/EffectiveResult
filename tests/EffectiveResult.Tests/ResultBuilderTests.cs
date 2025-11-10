using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests;

public class ResultBuilderTests
{
    [Fact]
    public void ResultBuilderConstructor_WhenUseBuilder_ShouldNotThrowExceptions()
    {
        // Arrange

        // Act
        var builderAction = () => Result.Build();
        var builderFromStaticAction = () => ResultBuilder.Create();
        var builderWithCapacityAction = () => ResultBuilder.Create(0);

        // Assert
        builderAction.Should().NotThrow();
        builderFromStaticAction.Should().NotThrow();
        builderWithCapacityAction.Should().NotThrow();
    }

    [Fact]
    public void AppendError_WhenAppendErrorsWithDifferentTypes_ShouldCreateValidResult()
    {
        // Arrange
        var builder = Result.Build();
        var errorMessage = "Bad situation";
        var error = new ResultError("Bad data");
        var exception = new Exception("Oops");

        // Act
        builder.AppendError(error);
        builder.AppendError(exception);
        builder.AppendError(errorMessage);
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(error, new ResultError(exception), new ResultError(errorMessage));
    }

    [Fact]
    public void AppendErrors_WhenAppendErrorsWithDifferentTypes_ShouldCreateValidResult()
    {
        // Arrange
        var builder = Result.Build();
        var errorMessage = "Bad situation";
        var exception = new Exception("Oops");
        var error = new ResultError("Bad data");

        // Act
        builder.AppendErrors(error, new ResultError(exception), new ResultError(errorMessage));
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(error, new ResultError(exception), new ResultError(errorMessage));
    }

    [Fact]
    public void AppendErrors_WhenAppendErrorsAsEnumerableWithDifferentTypes_ShouldCreateValidResult()
    {
        // Arrange
        var builder = Result.Build();
        var errorMessage = "Bad situation";
        var exception = new Exception("Oops");
        var error = new ResultError("Bad data");

        // Act
        builder.AppendErrors(new[] { error, new ResultError(exception), new ResultError(errorMessage) }.AsEnumerable());
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(error, new ResultError(exception), new ResultError(errorMessage));
    }

    [Fact]
    public void Create_WhenAppendErrorsAsArrayWithDifferentTypes_ShouldCreateValidResult()
    {
        // Arrange
        var errorMessage = "Bad situation";
        var exception = new Exception("Oops");
        var error = new ResultError("Bad data");

        // Act
        var builder = ResultBuilder.Create(error, new ResultError(exception), new ResultError(errorMessage));
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(error, new ResultError(exception), new ResultError(errorMessage));
    }

    [Fact]
    public void Create_WhenAppendErrorsAsEnumerableWithDifferentTypes_ShouldCreateValidResult()
    {
        // Arrange
        var errorMessage = "Bad situation";
        var exception = new Exception("Oops");
        var error = new ResultError("Bad data");
        var errorsEnumerable = new[]
        {
            error,
            new ResultError(exception),
            new ResultError(errorMessage)
        }.AsEnumerable();

        // Act
        var builder = ResultBuilder.Create(errorsEnumerable);
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(error, new ResultError(exception), new ResultError(errorMessage));
    }

    [Fact]
    public void AppendStateOf_WhenAppendFailedResult_ShouldCreateValidResult()
    {
        // Arrange
        var builder = Result.Build();
        var errorMessage = "Bad situation";
        var errorMessage2 = "Very bad situation";
        var error = new ResultError("Bad data");
        var exception = new Exception("Oops");

        // Act
        builder.AppendStateOf(Result.Fail(error));
        builder.AppendStateOf(Result.Fail(exception));
        builder.AppendStateOf(Result.Fail(errorMessage));
        builder.AppendStateOf(Result.Fail<int>(errorMessage2));
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(
            error,
            new ResultError(exception),
            new ResultError(errorMessage),
            new ResultError(errorMessage2));
    }

    [Fact]
    public void AppendStateOf_WhenAppendOtherResultBuilder_ShouldCreateValidResult()
    {
        // Arrange
        var builder = Result.Build();
        var errorMessage = "Bad situation";
        var builderWithErrors = ResultBuilder.Create(new ResultError(errorMessage));
        var builderWithoutErrors = ResultBuilder.Create();

        // Act
        builder.AppendStateOf(builderWithErrors);
        builder.AppendStateOf(builderWithoutErrors);
        var result = builder.ToResult();

        // Assert
        result.ShouldBeFailed(new ResultError(errorMessage));
    }

    [Fact]
    public void ToResult_WhenBuilderWithErrors_ShouldCreateFailedResult()
    {
        // Arrange
        var errorMessage = "Bad situation";
        var builder = ResultBuilder.Create(new ResultError(errorMessage));

        // Act
        var result = builder.ToResult();
        var resultWithValue = builder.ToResult(123);
        var resultWithValueFactory = builder.ToResult(() => 456);

        // Assert
        result.ShouldBeFailed(new ResultError(errorMessage));
        resultWithValue.ShouldBeFailed(new ResultError(errorMessage));
        resultWithValueFactory.ShouldBeFailed(new ResultError(errorMessage));
    }

    [Fact]
    public void ToResult_WhenBuilderWithoutErrors_ShouldCreateSuccessResult()
    {
        // Arrange
        var builder = ResultBuilder.Create();
        var value1 = 123;
        var value2 = 3456;

        // Act
        var result = builder.ToResult();
        var resultWithValue = builder.ToResult(value1);
        var resultWithValueFactory = builder.ToResult(() => value2);

        // Assert
        result.ShouldBeSuccess();
        resultWithValue.ShouldBeSuccess();
        resultWithValueFactory.ShouldBeSuccess();
    }
}
