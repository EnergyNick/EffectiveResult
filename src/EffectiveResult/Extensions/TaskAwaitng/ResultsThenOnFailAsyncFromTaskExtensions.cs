using System.Diagnostics.CodeAnalysis;

namespace EffectiveResult.Extensions;

[ExcludeFromCodeCoverage(
    Justification =
        "All methods use other covered extension method, "
        + "here it`s just awaiting task and passing result to the next method")]
public static class ResultsThenOnFailAsyncFromTaskExtensions
{
    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Task<Result> input, Action onFailAction)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(onFailAction);
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(
        this Task<Result> input,
        Action<IEnumerable<ResultError>> onFailAction)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(onFailAction);
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static async Task<Result<TValue>>ThenOnFailAsync<TValue>(
        this Task<Result<TValue>> input,
        Func<IReadOnlyCollection<ResultError>, TValue> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(continuation);
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
        Func<IReadOnlyCollection<ResultError>, Result<TValue>> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(continuation);
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
        Func<TValue> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(continuation);
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
        Func<Result<TValue>> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFail(continuation);
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
        Action<ResultError> onFailAction)
        where TException : Exception
    {
        var inputResult = await input.ConfigureAwait(false);
        return inputResult.ThenOnFailWithException<TException>(onFailAction);
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Task<Result> input, Func<Task> onFailAction)
    {
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(onFailAction);
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static async Task<Result> ThenOnFailAsync(this Task<Result> input, Func<IEnumerable<ResultError>, Task> onFailAction)
    {
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(onFailAction);
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
        Func<IReadOnlyCollection<ResultError>, Task<TValue>> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(continuation);
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
        Func<IReadOnlyCollection<ResultError>, Task<Result<TValue>>> continuation)
    {
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(continuation);
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
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(continuation);
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
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailAsync(continuation);
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
        Func<ResultError, Task> onFailAction)
        where TException : Exception
    {
        var inputResult = await input.ConfigureAwait(false);
        return await inputResult.ThenOnFailWithExceptionAsync<TException>(onFailAction);
    }
}
