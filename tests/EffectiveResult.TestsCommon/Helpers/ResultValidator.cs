using EffectiveResult.Abstractions;
using EffectiveResult.Exceptions;
using FluentAssertions;

namespace EffectiveResult.TestsCommon.Helpers;

public static class ResultValidator
{
    public static void ShouldBeSuccess(this IConclusion result)
    {
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    public static void ShouldBeSuccess<TResult, TValue>(this TResult result)
        where TResult : IConclusion, IValueStorage<TValue>
    {
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        result.Errors.Should().BeEmpty();

        result.Invoking(x => x.Value).Should().NotThrow();
    }

    public static void ShouldBeSuccessAndEqualsValue<TResult, TValue>(this TResult result, TValue expected)
        where TResult : IConclusion, IValueStorage<TValue>
    {
        result.ShouldBeSuccess();

        result.ValueOrDefault.Should().Be(expected);
        result.Value.Should().Be(expected);
    }

    public static void ShouldBeSuccessAndValueCollectionEqualsTo<TResult, TValue>(this TResult result,
        ICollection<TValue> expected)
        where TResult : IConclusion, IValueStorage<IEnumerable<TValue>>
    {
        result.ShouldBeSuccess();

        result.ValueOrDefault.Should().BeEquivalentTo(expected);
        result.Value.Should().BeEquivalentTo(expected);
    }

    public static void ShouldBeSuccessAndReferenceEqualsValue<TResult, TValue>(this TResult result, TValue expected)
        where TValue : class
        where TResult : IConclusion, IValueStorage<TValue>
    {
        result.ShouldBeSuccessAndEqualsValue(expected);
        ReferenceEquals(result.Value, expected).Should().BeTrue();
    }

    public static void ShouldBeFailed(this IConclusion result, params IError[] expectedErrors) =>
        result.ShouldBeFailed(expectedErrors as ICollection<IError>);

    public static void ShouldBeFailed(this IConclusion result, IEnumerable<IError>? expectedErrors = null)
    {
        result.IsSuccess.Should().BeFalse();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().NotBeEmpty();

        var expectedErrorsArray = expectedErrors?.ToArray();
        if (expectedErrorsArray is { Length: > 0 })
            result.Errors.Should().BeEquivalentTo(expectedErrorsArray);
    }

    public static void ShouldBeFailed<TResult, TValue>(this TResult result, params IError[] expectedErrors)
        where TResult : IConclusion, IValueStorage<TValue> =>
        result.ShouldBeFailed(expectedErrors as ICollection<IError>);

    public static void ShouldBeFailed<TResult, TValue>(this TResult result, IEnumerable<IError>? expectedErrors = null)
        where TResult : IConclusion, IValueStorage<TValue>
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