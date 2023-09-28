using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> Then<TOutput>(this Result input, Func<TOutput> continuation)
    {
        return input.IsSuccess
            ? continuation().MakeResult()
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result Then(this Result input, Func<Result> continuation)
    {
        return input.IsSuccess
            ? continuation()
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> Then<TOutput>(this Result input, Func<Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation()
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        return input.IsSuccess
            ? continuation(input.ValueOrDefault!).MakeResult()
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result Then<TInput>(this Result<TInput> input,
        Func<TInput, Result> continuation)
    {
        return input.IsSuccess
            ? continuation(input.ValueOrDefault!)
            : input.ToResult();
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> Then<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        return input.IsSuccess
            ? continuation(input.ValueOrDefault!)
            : input.ToResult<TOutput>();
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
            ? continuation(input.Errors).MakeResult()
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
            ? continuation().MakeResult()
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
}