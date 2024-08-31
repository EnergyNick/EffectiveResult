using FunctionalResult.Extensions;
using FunctionalResult.TestsCommon.Helpers;

namespace FunctionalResult.Tests.Extensions;

public class ObjectExtensionTests
{
    [Fact]
    public void MakeResult_WhenInvokeWithValue_ShouldReturnValidSuccessResult()
    {
        // Arrange
        var data = "This is not only object!";

        // Act
        var result = data.MakeResult();

        // Assert
        result.ShouldBeSuccessAndReferenceEqualsValue(data);
    }

    [Fact]
    public void MakeFailedResult_WhenInvokeOnErrorObject_ShouldReturnFailedResultWithSameError()
    {
        // Arrange
        var error = new Error("Very awful situation");

        // Act
        var result = error.MakeFailedResult();
        var resultTyped = error.MakeFailedResult<string>();

        // Assert
        result.ShouldBeFailed(error);
        resultTyped.ShouldBeFailed(error);
    }

    [Fact]
    public void MakeFailedResult_WhenInvokeOnExceptionObject_ShouldReturnFailedResultWithValidError()
    {
        // Arrange
        var exception = new Exception("Very awful situation");
        var error = new ExceptionalError(exception);

        // Act
        var result = exception.MakeFailedResult();
        var resultTyped = exception.MakeFailedResult<string>();

        // Assert
        result.ShouldBeFailed(error);
        resultTyped.ShouldBeFailed(error);
    }
}