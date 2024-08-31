using System.Diagnostics.CodeAnalysis;
using SimpleResult.Core;
using SimpleResult.Exceptions;
using SimpleResult.TestsCommon.Helpers;

namespace SimpleResult.Tests;

[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
public class ResultContractTests
{
    [Fact]
    public void ResultConstruction_WhenCreateSuccessResult_ShouldBeInValidState()
    {
        // Arrange
        var valueInt = 5;
        var valueStr = new List<string> { "Wow!" };

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
        IError error = new Error("Very bad");

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

        // Act
        var resultArray = () => new Result(errors);
        var valuedResultArray = () => new Result<List<string>>(errors);

        // Assert
        resultArray.Should().Throw<InvalidResultOperationException>();
        valuedResultArray.Should().Throw<InvalidResultOperationException>();
    }

    [Fact]
    public void ResultConstruction_WhenCloneWithCopyConstructor_ShouldBeEquals()
    {
        // Arrange
        IError error = new Error("So bad");
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
    public void ResultGetValueOrDefault_WhenInvokeOnSuccessResult_ShouldReturnValue()
    {
        // Arrange
        var data = new List<string> { "Wow!" };
        var result = new Result<List<string>>(data);

        var defaultValue = new List<string> { "Default" };
        var defaultValueFromFactory = new List<string> { "Factory" };

        // Act
        var valueOrDefault = result.GetValueOrDefault(defaultValue);
        var valueOrDefaultFactory = result.GetValueOrDefault(() => defaultValueFromFactory);

        // Assert
        valueOrDefault.Should().BeEquivalentTo(data);
        valueOrDefaultFactory.Should().BeEquivalentTo(data);
    }

    [Fact]
    public void ResultGetValueOrDefault_WhenInvokeOnFailedResult_ShouldReturnDefaultValue()
    {
        // Arrange
        var error = new Error("Error!!!");
        var result = Result.Fail<List<string>>(error);

        var defaultValue = new List<string> { "Default" };
        var defaultValueFromFactory = new List<string> { "Factory" };

        // Act
        var valueOrDefault = result.GetValueOrDefault(defaultValue);
        var valueOrDefaultFactory = result.GetValueOrDefault(() => defaultValueFromFactory);

        // Assert
        valueOrDefault.Should().BeEquivalentTo(defaultValue);
        valueOrDefaultFactory.Should().BeEquivalentTo(defaultValueFromFactory);
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

    [Fact]
    public void ResultImplicitOperator_WhenSetFromValue_ShouldCreateSuccessResult()
    {
        // Arrange
        var valueInt = 5;
        var valueStr = new List<string> { "Wow!" };

        // Act
        Result<int> valuedResult = valueInt;
        Result<List<string>> classResult = valueStr;

        // Assert
        valuedResult.ShouldBeSuccessAndEqualsValue(valueInt);
        classResult.ShouldBeSuccessAndReferenceEqualsValue(valueStr);
    }

    [Fact]
    public void ResultImplicitOperator_WhenSetFromError_ShouldCreateFailedResult()
    {
        // Arrange
        var error = new Error("Very bad");

        // Act
        Result result = error;
        Result<int> valuedResult = error;
        Result<List<string>> classResult = error;

        // Assert
        result.ShouldBeFailed(error);
        valuedResult.ShouldBeFailed(error);
        classResult.ShouldBeFailed(error);
    }

    [Fact]
    public void ResultDeconstructOperator_WhenInvoked_ShouldReturnValidData()
    {
        // Arrange
        var error = new Error("Very bad");
        var strData = "Hello there!";
        var valueData = 1988;

        var result = Result.Fail(error);
        var classResult = Result.Ok(strData);
        var valuedResult = Result.Ok(valueData);

        // Act
        var (isSuccess1, errors1) = result;
        var (isSuccess2, errors2) = valuedResult;
        var (isSuccess3, valueOrDefault, errors3) = classResult;

        // Assert
        isSuccess1.Should().BeFalse();
        errors1.Should().ContainSingle(x => error.Equals(x));

        isSuccess2.Should().BeTrue();
        errors2.Should().BeEmpty();

        isSuccess3.Should().BeTrue();
        errors3.Should().BeEmpty();
        valueOrDefault.Should().Be(strData);
    }
}