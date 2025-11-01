namespace EffectiveResult.Extensions;

public static partial class ResultsThenAsyncExtensions
{
    /// <summary>
    /// Provide chaining method for async action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async action for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync(this Task<Result> input, Func<Task> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await Result.TryAsync(continuation)
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync<TValue>(this Task<Result<TValue>> input, Func<TValue, Task> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await Result.TryAsync(() => continuation(inputResult.Value))
            : inputResult.ToResult();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TOutput>(this Task<Result> input, Func<Task<TOutput>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await Result.TryAsync(continuation)
            : inputResult.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync(this Task<Result> input, Func<Task<Result>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await continuation()
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result<TOutput>> ThenAsync<TOutput>(this Task<Result> input, Func<Task<Result<TOutput>>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await continuation()
            : inputResult.ToResult<TOutput>();
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
        this Task<Result<TInput>> input,
        Func<TInput, Task<TOutput>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await Result.TryAsync(() => continuation(inputResult.Value))
            : inputResult.ToResult<TOutput>();
    }

    /// <summary>
    /// Provide chaining method for next async operation on success result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Async function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static async Task<Result> ThenAsync<TInput>(
        this Task<Result<TInput>> input,
        Func<TInput, Task<Result>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await continuation(inputResult.Value)
            : inputResult.ToResult();
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
        this Task<Result<TInput>> input,
        Func<TInput, Task<Result<TOutput>>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsSuccess
            ? await continuation(inputResult.Value)
            : inputResult.ToResult<TOutput>();
    }
}