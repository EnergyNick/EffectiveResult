using System.Diagnostics.CodeAnalysis;
using SimpleResult.Core;
using SimpleResult.Exceptions;
using SimpleResult.Tests.Helpers;

namespace SimpleResult.Tests;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class ResultContractTests
{
    [Fact]
    public void ResultConstruction_WhenCreateSuccessResult_ShouldBeInValidState()
    {
        // Arrange
        var valueInt = 5;
        var valueStr = new List<string> {"Wow!"};

        // Act
        var result = new Result();
        var valuedResult = new Result<int>(valueInt);
        var classResult = new Result<List<string>>(valueStr);

        // Assert
        result.ShouldBeSuccess();
        valuedResult.ShouldBeSuccessAndEqualsValue(valueInt);
        classResult.ShouldBeSuccessAndReferenceEqualsValue(valueStr);
    }

    [Fact]
    public void ResultConstruction_WhenCreateFailedResult_ShouldBeInValidState()
    {
        // Arrange
        var error = new Error("Very bad");

        // Act
        var result = new Result(error);
        var valuedResult = new Result<int>(error);
        var classResult = new Result<List<string>>(error);

        // Assert
        result.ShouldBeFailed(error);
        valuedResult.ShouldBeFailed(error);
        classResult.ShouldBeFailed(error);
    }

    [Fact]
    public void ResultConstruction_WhenCreateFailedResultWithErrors_ShouldBeInValidState()
    {
        // Arrange
        var errors = new IError[]
        {
            new Error("Very bad"),
            new ExceptionalError(new Exception())
        };

        // Act
        var result = new Result(errors);
        var valuedResult = new Result<int>(errors);
        var classResult = new Result<List<string>>(errors);

        // Assert
        result.ShouldBeFailed(errors);
        valuedResult.ShouldBeFailed(errors);
        classResult.ShouldBeFailed(errors);
    }

    [Fact]
    public void ResultConstruction_WhenCreateFailedResultWithNoErrors_ShouldBeThrown()
    {
        // Arrange
        var errors = Array.Empty<IError>();
        var enumerableErrors = errors.AsEnumerable();

        // Act
        var resultArray = () => new Result(errors);
        var resultEnumerable = () => new Result(enumerableErrors);
        var valuedResultArray = () => new Result<List<string>>(errors);
        var valuedResultEnumerable = () => new Result<List<string>>(enumerableErrors);

        // Assert
        resultArray.Should().Throw<InvalidResultOperationException>();
        resultEnumerable.Should().Throw<InvalidResultOperationException>();
        valuedResultArray.Should().Throw<InvalidResultOperationException>();
        valuedResultEnumerable.Should().Throw<InvalidResultOperationException>();
    }

    [Fact]
    public void ResultConstruction_WhenCreateFailedResultWithEnumerableErrors_ShouldBeInValidState()
    {
        // Arrange
        var errors = new IError[]
        {
            new Error("Very bad"),
            new ExceptionalError(new Exception())
        }.AsEnumerable();

        // Act
        var result = new Result(errors);
        var valuedResult = new Result<int>(errors);
        var classResult = new Result<List<string>>(errors);

        // Assert
        result.ShouldBeFailed(errors);
        valuedResult.ShouldBeFailed(errors);
        classResult.ShouldBeFailed(errors);
    }

    [Fact]
    public void ResultConstruction_WhenCloneWithCopyConstructor_ShouldBeEquals()
    {
        // Arrange
        var error = new Error("So bad");
        var value = new List<int> { 1, 2, 3, 4, 5 };

        var successResult = new Result<List<int>>(value);
        var failedResult = new Result(error);

        // Act
        var successCopy = new Result<List<int>>(successResult);
        var failedCopy = new Result(failedResult);

        // Assert
        successCopy.Should().BeEquivalentTo(successResult);
        failedCopy.Should().BeEquivalentTo(failedResult);
    }

    [Fact]
    public void ToResult_WhenConvertToValuelessResult_StateShouldBeEquals()
    {
        // Arrange
        var structValue = 5;
        var classValue = new List<int> { 1, 2, 3, 4, 5 };

        var structResult = new Result<int>(structValue);
        var classResult = new Result<List<int>>(classValue);

        // Act
        var structResultCopy = structResult.ToResult();
        var classResultCopy = classResult.ToResult();

        // Assert
        structResultCopy.ShouldBeSuccess();
        classResultCopy.ShouldBeSuccess();
    }

    [Fact]
    public void ToResult_WhenConvertSuccessResultToValueResult_StateShouldBeEqualsAndContainsValue()
    {
        // Arrange
        var structValue = 5;
        var classValue = new List<int> { 1, 2, 3, 4, 5 };

        var result = new Result();
        var structResult = new Result<int>(structValue);
        var classResult = new Result<List<int>>(classValue);

        var newValue = 500;
        var newValueFactory = () => newValue;
        var newList = new List<string> { "Hello!" };

        // Act
        var resultCopy = result.ToResult(newValue);
        var resultFactoryCopy = result.ToResult(newValueFactory);
        var structResultCopy = classResult.ToResult(_ => newValue);
        var classResultCopy = structResult.ToResult(_ => newList);

        // Assert
        resultCopy.ShouldBeSuccessAndEqualsValue(newValue);
        resultFactoryCopy.ShouldBeSuccessAndEqualsValue(newValue);
        structResultCopy.ShouldBeSuccessAndEqualsValue(newValue);
        classResultCopy.ShouldBeSuccessAndReferenceEqualsValue(newList);
    }

    [Fact]
    public void ToResult_WhenConvertFailedResultToValueResult_StateShouldBeEqualsAndContainsValue()
    {
        // Arrange
        var data = 145;

        var error = new Error("Bad data");
        var errors = new IError[] { error };

        var result = new Result(errors);
        var structResult = new Result<int>(errors);
        var classResult = new Result<List<int>>(errors);

        // Act
        var resultCopyAction = () => result.ToResult(data);
        var resultValueFactoryCopyAction = () => result.ToResult<string>();
        var structResultCopyAction = () => classResult.ToResult<int>();
        var classResultCopyAction = () => structResult.ToResult<string>();

        // Assert
        resultCopyAction.Should().NotThrow();
        resultValueFactoryCopyAction.Should().NotThrow();
        structResultCopyAction.Should().NotThrow();
        classResultCopyAction.Should().NotThrow();

        resultCopyAction().ShouldBeFailed(errors);
        resultValueFactoryCopyAction().ShouldBeFailed(errors);
        structResultCopyAction().ShouldBeFailed(errors);
        classResultCopyAction().ShouldBeFailed(errors);
    }

    [Fact]
    public void ToResult_WhenConvertSuccessResultToValueResultWithoutConverterOrValue_ShouldThrow()
    {
        // Arrange
        const string nullStr = null!;
        var structValue = 5;
        var classValue = new List<int> { 1, 2, 3, 4, 5 };

        var result = new Result();
        var structResult = new Result<int>(structValue);
        var classResult = new Result<List<int>>(classValue);

        // Act
        var resultCopyAction = () => result.ToResult<int>();
        var resultFactoryCopyAction = () => result.ToResult(nullStr);
        var structResultCopyAction = () => classResult.ToResult<int>();
        var classResultCopyAction = () => structResult.ToResult<string>();

        // Assert
        resultCopyAction.Should().Throw<ArgumentNullOnSuccessException>();
        resultFactoryCopyAction.Should().Throw<ArgumentNullOnSuccessException>();
        structResultCopyAction.Should().Throw<ArgumentNullOnSuccessException>();
        classResultCopyAction.Should().Throw<ArgumentNullOnSuccessException>();
    }
}