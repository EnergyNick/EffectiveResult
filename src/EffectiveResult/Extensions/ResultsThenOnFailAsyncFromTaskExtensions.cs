using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static partial class ResultsThenOnFailAsyncExtensions
{
    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Task<Result> input, Func<Task> onFailAction)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await Result.TryAsync(onFailAction)
            : inputResult;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Task<Result> input, Func<IEnumerable<IError>, Task> onFailAction)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await Result.TryAsync(() => onFailAction(inputResult.Errors))
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Task<Result<TValue>> input,
        Func<IReadOnlyCollection<IError>, Task<TValue>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await Result.TryAsync(() => continuation(inputResult.Errors))
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Task<Result<TValue>> input,
        Func<IReadOnlyCollection<IError>, Task<Result<TValue>>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await continuation(inputResult.Errors)
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Task<Result<TValue>> input,
        Func<Task<TValue>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await Result.TryAsync(continuation)
            : inputResult;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Task<Result<TValue>> input,
        Func<Task<Result<TValue>>> continuation)
    {
        var inputResult = await input;
        return inputResult.IsFailed
            ? await continuation()
            : inputResult;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed and contains exception with type <see cref="TException"/>
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TException">Type of searching error</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailWithExceptionAsync<TException>(
        this Task<Result> input,
        Func<IExceptionalError, Task> onFailAction)
        where TException : Exception
    {
        var inputResult = await input;
        if (inputResult.IsFailed)
        {
            var exceptionalError = inputResult.Errors
                .OfType<IExceptionalError>()
                .FirstOrDefault(x => x.Exception is TException);

            if (exceptionalError is not null)
                return await Result.TryAsync(() => onFailAction(exceptionalError));
        }

        return inputResult;
    }
}