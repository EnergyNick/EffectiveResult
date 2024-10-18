using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ResultsThenOnFailExtensions
{
    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result ThenOnFail(this Result input, Action onFailAction)
    {
        return input.IsFailed
            ? Result.Try(onFailAction)
            : input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result ThenOnFail(this Result input, Action<IEnumerable<IError>> onFailAction)
    {
        return input.IsFailed
            ? Result.Try(() => onFailAction(input.Errors))
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenOnFail<TValue>(this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, TValue> continuation)
    {
        return input.IsFailed
            ? Result.Try(() => continuation(input.Errors))
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenOnFail<TValue>(this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, Result<TValue>> continuation)
    {
        return input.IsFailed
            ? continuation(input.Errors)
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenOnFail<TValue>(this Result<TValue> input, Func<TValue> continuation)
    {
        return input.IsFailed
            ? Result.Try(continuation)
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenOnFail<TValue>(this Result<TValue> input, Func<Result<TValue>> continuation)
    {
        return input.IsFailed
            ? continuation()
            : input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed and contains exception with type <see cref="TException"/>
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TException">Type of searching error</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result ThenOnFailWithException<TException>(this Result input,
        Action<IExceptionalError> onFailAction)
        where TException : Exception
    {
        if (input.IsFailed)
        {
            var exceptionalError = input.Errors
                .OfType<IExceptionalError>()
                .FirstOrDefault(x => x.Exception is TException);

            if (exceptionalError is not null)
                return Result.Try(() => onFailAction(exceptionalError));
        }

        return input;
    }
}