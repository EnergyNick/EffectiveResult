using SimpleResult.Core;
using SimpleResult.Exceptions;

namespace SimpleResult.Tests.Helpers;

public static class ResultValidator
{
    public static void ShouldBeSuccess(this Result result)
    {
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    public static void ShouldBeSuccess<TValue>(this Result<TValue> result)
    {
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();

        result.Invoking(x => x.Value).Should().NotThrow();
    }

    public static void ShouldBeSuccessAndEqualsValue<TValue>(this Result<TValue> result, TValue expected)
    {
        result.ShouldBeSuccess();

        result.ValueOrDefault.Should().Be(expected);
        result.Value.Should().Be(expected);
    }

    public static void ShouldBeSuccessAndValueCollectionEqualsTo<TValue>(this Result<IEnumerable<TValue>> result,
        ICollection<TValue> expected)
    {
        result.ShouldBeSuccess();

        result.ValueOrDefault.Should().BeEquivalentTo(expected);
        result.Value.Should().BeEquivalentTo(expected);
    }

    public static void ShouldBeSuccessAndReferenceEqualsValue<TValue>(this Result<TValue> result, TValue expected)
        where TValue : class
    {
        result.ShouldBeSuccessAndEqualsValue(expected);
        ReferenceEquals(result.Value, expected).Should().BeTrue();
    }

    public static void ShouldBeFailed(this Result result, params IError[] expectedErrors) =>
        result.ShouldBeFailed(expectedErrors as ICollection<IError>);

    public static void ShouldBeFailed(this Result result, IEnumerable<IError>? expectedErrors = null)
    {
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();

        var expectedErrorsArray = expectedErrors?.ToArray();
        if (expectedErrorsArray is { Length: > 0 })
            result.Errors.Should().BeEquivalentTo(expectedErrorsArray);
    }

    public static void ShouldBeFailed<TValue>(this Result<TValue> result, params IError[] expectedErrors) =>
        result.ShouldBeFailed(expectedErrors as ICollection<IError>);

    public static void ShouldBeFailed<TValue>(this Result<TValue> result, IEnumerable<IError>? expectedErrors = null)
    {
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();

        var expectedErrorsArray = expectedErrors?.ToArray();
        if (expectedErrorsArray is { Length: > 0 })
            result.Errors.Should().BeEquivalentTo(expectedErrorsArray);

        result.Invoking(x => x.Value).Should().Throw<OperationOnFailedResultException>();
    }
}