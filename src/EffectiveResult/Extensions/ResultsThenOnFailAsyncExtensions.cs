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
    public static async Task<Result> ThenOnFailAsync(this Result input, Func<Task> onFailAction)
    {
        return input.IsFailed
            ? await Result.TryAsync(onFailAction)
            : input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Result input, Func<IEnumerable<IError>, Task> onFailAction)
    {
        return input.IsFailed
            ? await Result.TryAsync(() => onFailAction(input.Errors))
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, Task<TValue>> continuation)
    {
        return input.IsFailed
            ? await Result.TryAsync(() => continuation(input.Errors))
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, Task<Result<TValue>>> continuation)
    {
        return input.IsFailed
            ? await continuation(input.Errors)
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Result<TValue> input,
        Func<Task<TValue>> continuation)
    {
        return input.IsFailed
            ? await Result.TryAsync(continuation)
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>> ThenOnFailAsync<TValue>(
        this Result<TValue> input,
        Func<Task<Result<TValue>>> continuation)
    {
        return input.IsFailed
            ? await continuation()
            : input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed and contains exception with type <see cref="TException"/>
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TException">Type of searching error</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailWithExceptionAsync<TException>(
        this Result input,
        Func<IExceptionalError, Task> onFailAction)
        where TException : Exception
    {
        if (input.IsFailed)
        {
            var exceptionalError = input.Errors
                .OfType<IExceptionalError>()
                .FirstOrDefault(x => x.Exception is TException);

            if (exceptionalError is not null)
                return await Result.TryAsync(() => onFailAction(exceptionalError));
        }

        return input;
    }
}