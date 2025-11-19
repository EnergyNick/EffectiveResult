using System.Text.Json;

namespace EffectiveResult.Tests;

public class ResultSerializationTests
{
    [Fact]
    public async Task ResultSerialize_WhenSerializeAndDeserialize_ShouldBeSameValues()
    {
        // Arrange
        var valueInt = 5;
        var valueStr = new List<string> { "Wow!" };

        var result = new Result();
        var resultWithStructure = new Result<int>(valueInt);
        var resultWithReference = new Result<List<string>>(valueStr);
        var failedResult = Result.Fail(new ResultError("Badest", new ResultError("Other")));
        var failedResultWithStructure = Result.Fail<int>("Badest");
        var failedResultWithReference = Result.Fail<string>("Badest");

        // Act
        var resultStr = JsonSerializer.Serialize(result);
        var resultWithStructureStr = JsonSerializer.Serialize(resultWithStructure);
        var resultWithReferenceStr = JsonSerializer.Serialize(resultWithReference);
        var failedResultStr = JsonSerializer.Serialize(failedResult);
        var failedResultWithStructureStr = JsonSerializer.Serialize(failedResultWithStructure);
        var failedResultWithReferenceStr = JsonSerializer.Serialize(failedResultWithReference);

        var resultObj = JsonSerializer.Deserialize<Result>(resultStr);
        var resultWithStructureObj = JsonSerializer.Deserialize<Result<int>>(resultWithStructureStr);
        var resultWithReferenceObj = JsonSerializer.Deserialize<Result<List<string>>>(resultWithReferenceStr);
        var failedResultObj = JsonSerializer.Deserialize<Result>(failedResultStr);
        var failedResultWithStructureObj = JsonSerializer.Deserialize<Result<int>>(failedResultWithStructureStr);
        var failedResultWithReferenceObj = JsonSerializer.Deserialize<Result<string>>(failedResultWithReferenceStr);

        // Assert
        Assert.NotNull(resultObj);
        Assert.NotNull(resultWithStructureObj);
        Assert.NotNull(resultWithReferenceObj);
        Assert.NotNull(failedResultObj);
        Assert.NotNull(failedResultWithStructureObj);
        Assert.NotNull(failedResultWithReferenceObj);

        resultObj.Should().Be(result);
        resultWithStructureObj.Should().Be(resultWithStructure);
        resultWithReferenceObj.IsSuccess.Should().Be(resultWithReference.IsSuccess);
        resultWithReferenceObj.ValueOrDefault.Should().BeEquivalentTo(resultWithReference.ValueOrDefault);
        failedResultObj.Should().Be(failedResult);
        failedResultWithStructureObj.Should().Be(failedResultWithStructure);
        failedResultWithReferenceObj.Should().Be(failedResultWithReference);
    }
}
