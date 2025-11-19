using System.Diagnostics.CodeAnalysis;
using EffectiveResult.Exceptions;
using EffectiveResult.TestsCommon.Helpers;

namespace EffectiveResult.Tests;

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
        var error = new ResultError("Very bad");

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
        var errors = new ResultError[]
        {
            new("Very bad"),
            new(new Exception())
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
        var errors = Array.Empty<ResultError>();

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
        var error = new ResultError("So bad");
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
        var error = new ResultError("Error!!!");
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

        var error = new ResultError("Bad data");
        var errors = new ResultError[] { error };

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
        resultCopyAction.Should().NotThrow();
        resultFactoryCopyAction.Should().NotThrow();
        structResultCopyAction.Should().NotThrow();
        classResultCopyAction.Should().NotThrow();
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
        var error = new ResultError("Very bad");

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
        var error = new ResultError("Very bad");
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

    [Fact]
    public void ResultEqualsOperator_WhenCompareSuccessResultsWithValue_ShouldReturnValidState()
    {
        // Arrange
        var data = 123;
        var otherDate = 234;

        var result = Result.Ok(data);
        var resultSame = Result.Ok(data);
        var resultDifferent = Result.Ok(otherDate);

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
    public void ResultEqualsOperator_WhenCompareSuccessResultsWithRef_ShouldReturnValidState()
    {
        // Arrange
        var data = "Testing";
        var otherDate = "Different";

        var result = Result.Ok(data);
        var resultSame = Result.Ok(data);
        var resultDifferent = Result.Ok(otherDate);

        // Act
        var equalResultForSame = result.Equals(result);
        var equalResultForEquivalent = result.Equals(resultSame);
        var equalResultForDifferent = result.Equals(resultDifferent);

        var equalResultForSameByOperator = result == result;
        var equalResultForEquivalentByOperator = result == resultSame;
        var equalResultForDifferentByOperator = result == resultDifferent;

        // Assert
        equalResultForSame.Should().BeTrue();
        equalResultForEquivalent.Should().BeTrue();
        equalResultForDifferent.Should().BeFalse();

        equalResultForSameByOperator.Should().BeTrue();
        equalResultForEquivalentByOperator.Should().BeTrue();
        equalResultForDifferentByOperator.Should().BeFalse();
    }

    [Fact]
    public void ResultEqualsOperator_WhenCompareSuccessAndFailedResults_ShouldReturnValidState()
    {
        // Arrange
        var result = Result.Ok();
        var resultFailed = Result.Fail("Bad");
        var resultWithValue = Result.Ok("Testing");
        var resultWithValueFailed = Result.Fail<string>("Bad");

        // Act
        var equalResult = result.Equals(resultFailed);
        var equalResultWithValue = resultWithValue.Equals(resultWithValueFailed);
        var equalResultByOperator = result == resultFailed;
        var equalResultWithValueByOperator = resultWithValue == resultWithValueFailed;

        // Assert
        equalResult.Should().BeFalse();
        equalResultWithValue.Should().BeFalse();
        equalResultByOperator.Should().BeFalse();
        equalResultWithValueByOperator.Should().BeFalse();
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
    public void ResultEqualsOperator_WhenCompareWithInvalid_ShouldReturnValidState()
    {
        // Arrange
        var result = Result.Ok();
        var resultWithValue = Result.Ok(123);

        // Act
        var equalResultForNull = result.Equals((object?)null);
        var equalResultForNullResult = result.Equals((Result?)null);
        var equalResultWithValueForNull = resultWithValue.Equals((object?)null);
        var equalResultWithValueForNullResult = resultWithValue.Equals((Result?)null);

        // Assert
        equalResultForNull.Should().BeFalse();
        equalResultForNullResult.Should().BeFalse();
        equalResultWithValueForNull.Should().BeFalse();
        equalResultWithValueForNullResult.Should().BeFalse();
    }
}
