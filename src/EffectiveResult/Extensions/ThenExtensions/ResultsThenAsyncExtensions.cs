namespace EffectiveResult.Extensions;

public static partial class ResultsThenAsyncExtensions
{
    /// <summary>
    /// Provide chaining method for async action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async action for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync(this Result input, Func<Task> continuation)
    {
        return input.IsSuccess
            ? await Result.TryAsync(continuation)
            : input;
    }

    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync<TValue>(this Result<TValue> input, Func<TValue, Task> continuation)
    {
        return input.IsSuccess
            ? await Result.TryAsync(() => continuation(input.Value))
            : input.ToResult();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TOutput>(this Result input, Func<Task<TOutput>> continuation)
    {
        return input.IsSuccess
            ? await Result.TryAsync(continuation)
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync(this Result input, Func<Task<Result>> continuation)
    {
        return input.IsSuccess
            ? await continuation()
            : input;
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TOutput>(this Result input, Func<Task<Result<TOutput>>> continuation)
    {
        return input.IsSuccess
            ? await continuation()
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Task<TOutput>> continuation)
    {
        return input.IsSuccess
            ? await Result.TryAsync(() => continuation(input.Value))
            : input.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync<TInput>(
        this Result<TInput> input,
        Func<TInput, Task<Result>> continuation)
    {
        return input.IsSuccess
            ? await continuation(input.Value)
            : input.ToResult();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, Task<Result<TOutput>>> continuation)
    {
        return input.IsSuccess
            ? await continuation(input.Value)
            : input.ToResult<TOutput>();
    }
}