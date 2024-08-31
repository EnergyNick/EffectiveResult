using SimpleResult.Extensions;
using SimpleResult.TestsCommon.Helpers;

namespace SimpleResult.Tests.Extensions;

public class FunctionalExtensionsTest
{
    [Fact]
    public void MergeInnerResult_WhenInvokeWithSuccessResults_ShouldReturnSuccessResult()
    {
        // Arrange
        var data = "Wow, join from Haskell!";
        var result = Result.Ok(Result.Ok());
        var valuedResult = Result.Ok(Result.Ok(data));

        // Act
        var mergedResult = result.MergeInnerResult();
        var mergedValueResult = valuedResult.MergeInnerResult();

        // Assert
        mergedResult.ShouldBeSuccess();
        mergedValueResult.ShouldBeSuccessAndEqualsValue(data);
    }

    [Fact]
    public void MergeInnerResult_WhenParentResultFailed_ShouldReturnFailedResultWithParentErrors()
    {
        // Arrange
        var error = new Error("succ pred not working!");
        var result = Result.Fail<Result>(error);
        var valuedResult = Result.Fail<Result<string>>(error);

        // Act
        var mergedResult = result.MergeInnerResult();
        var mergedValueResult = valuedResult.MergeInnerResult();

        // Assert
        mergedResult.ShouldBeFailed(error);
        mergedValueResult.ShouldBeFailed(error);
    }

    [Fact]
    public void MergeInnerResult_WhenInnerResultFailed_ShouldReturnFailedResultWithInnerErrors()
    {
        // Arrange
        var error = new Error("succ pred not working!");
        var result = Result.Ok(Result.Fail(error));
        var valuedResult = Result.Ok(Result.Fail<string>(error));

        // Act
        var mergedResult = result.MergeInnerResult();
        var mergedValueResult = valuedResult.MergeInnerResult();

        // Assert
        mergedResult.ShouldBeFailed(error);
        mergedValueResult.ShouldBeFailed(error);
    }
}