using System.Diagnostics.CodeAnalysis;
using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

[ExcludeFromCodeCoverage(
    Justification =
        "All methods use other covered extension method, "
        + "here it`s just awaiting task and passing result to the next method")]
public static class MatchFromTaskExtensions
{
    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this Task<TConclusion> input,
        Action onSuccessAction,
        Action onFailAction)
        where TConclusion : IConclusion
    {
        var inputResult = await input;
        inputResult.Match(onSuccessAction, onFailAction);
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
        this Task<TConclusion> input,
        Action<TValue> onSuccessAction,
        Action onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        var inputResult = await input;
        inputResult.Match(onSuccessAction, onFailAction);
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this Task<TConclusion> input,
        Action onSuccessAction,
        Action<IReadOnlyCollection<Error>> onFailAction)
        where TConclusion : IConclusion
    {
        var inputResult = await input;
        inputResult.Match(onSuccessAction, onFailAction);
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
        this Task<TConclusion> input,
        Action<TValue> onSuccessAction,
        Action<IReadOnlyCollection<Error>> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        var inputResult = await input;
        inputResult.Match(onSuccessAction, onFailAction);
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this Task<TConclusion> input,
        Func<Task> onSuccessAction,
        Func<Task> onFailAction)
        where TConclusion : IConclusion
    {
        var inputResult = await input;
        await inputResult.MatchAsync(onSuccessAction, onFailAction);
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
        this Task<TConclusion> input,
        Func<TValue, Task> onSuccessAction,
        Func<Task> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        var inputResult = await input;
        await inputResult.MatchAsync(onSuccessAction, onFailAction);
    }

    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static async Task MatchAsync<TConclusion>(
        this Task<TConclusion> input,
        Func<Task> onSuccessAction,
        Func<IReadOnlyCollection<Error>, Task> onFailAction)
        where TConclusion : IConclusion
    {
        var inputResult = await input;
        await inputResult.MatchAsync(onSuccessAction, onFailAction);
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
        this Task<TConclusion> input,
        Func<TValue, Task> onSuccessAction,
        Func<IReadOnlyCollection<Error>, Task> onFailAction)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        var inputResult = await input;
        await inputResult.MatchAsync(onSuccessAction, onFailAction);
    }
}
