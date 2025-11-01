namespace EffectiveResult.Extensions;

public static class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result Then(this Result input, Action continuation)
    {
        return input.IsSuccess
            ? Result.Try(continuation)
            : input;
    }

    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result Then<TValue>(this Result<TValue> input, Action<TValue> continuation)
    {
        return input.IsSuccess
            ? Result.Try(() => continuation(input.Value))
            : input.ToResult();
    }

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
            ? Result.Try(continuation)
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
            ? Result.Try(() => continuation(input.Value))
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
            ? continuation(input.Value)
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
            ? continuation(input.Value)
            : input.ToResult<TOutput>();
    }
}