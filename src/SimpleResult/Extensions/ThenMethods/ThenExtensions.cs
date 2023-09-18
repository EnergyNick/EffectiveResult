namespace SimpleResult.Extensions;

public static partial class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TValue> Then<TValue>(this Result<TValue> input, Action<TValue> continuation)
    {
        if (input.IsSuccess)
            continuation(input.ValueOrDefault!);
        return input;
    }

    /// <summary>
    /// Provide chaining method for next function on success result
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
    /// Provide chaining method for next function on success result
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
    /// Provide chaining method for next function on success result
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
    /// Provide chaining method for next function on success result
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
}