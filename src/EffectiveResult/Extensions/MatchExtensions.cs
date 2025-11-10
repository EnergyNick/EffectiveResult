using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class MatchExtensions
{
    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static void Match<TConclusion>(
        this TConclusion input,
        Action onSuccessAction,
        Action onFailAction)
        where TConclusion : IConclusion
    {
        if (input.IsSuccess)
        {
            onSuccessAction();
        }
        else
        {
            onFailAction();
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    public static void Match<TConclusion, TValue>(
        this TConclusion input,
        Action<TValue> onSuccessAction,
        Action onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        if (input.IsSuccess)
        {
            onSuccessAction(input.Value);
        }
        else
        {
            onFailAction();
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static void Match<TConclusion>(
        this TConclusion input,
        Action onSuccessAction,
        Action<IReadOnlyCollection<ResultError>> onFailAction)
        where TConclusion : IConclusion
    {
        if (input.IsSuccess)
        {
            onSuccessAction();
        }
        else
        {
            onFailAction(input.Errors);
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    public static void Match<TConclusion, TValue>(
        this TConclusion input,
        Action<TValue> onSuccessAction,
        Action<IReadOnlyCollection<ResultError>> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        if (input.IsSuccess)
        {
            onSuccessAction(input.Value);
        }
        else
        {
            onFailAction(input.Errors);
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this TConclusion input,
        Func<Task> onSuccessAction,
        Func<Task> onFailAction)
        where TConclusion : IConclusion
    {
        if (input.IsSuccess)
        {
            await onSuccessAction().ConfigureAwait(false);
        }
        else
        {
            await onFailAction().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    public static async Task MatchAsync<TConclusion, TValue>(
        this TConclusion input,
        Func<TValue, Task> onSuccessAction,
        Func<Task> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        if (input.IsSuccess)
        {
            await onSuccessAction(input.Value).ConfigureAwait(false);
        }
        else
        {
            await onFailAction().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this TConclusion input,
        Func<Task> onSuccessAction,
        Func<IReadOnlyCollection<ResultError>, Task> onFailAction)
        where TConclusion : IConclusion
    {
        if (input.IsSuccess)
        {
            await onSuccessAction().ConfigureAwait(false);
        }
        else
        {
            await onFailAction(input.Errors).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    public static async Task MatchAsync<TConclusion, TValue>(
        this TConclusion input,
        Func<TValue, Task> onSuccessAction,
        Func<IReadOnlyCollection<ResultError>, Task> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        if (input.IsSuccess)
        {
            await onSuccessAction(input.Value).ConfigureAwait(false);
        }
        else
        {
            await onFailAction(input.Errors).ConfigureAwait(false);
        }
    }
}
