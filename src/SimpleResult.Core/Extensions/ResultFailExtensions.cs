using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ResultFailExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static TResult Then<TResult>(this TResult input, Action continuation)
        where TResult : IConclusion
    {
        if (input.IsSuccess)
            continuation();

        return input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TResult">Type of conclusion</typeparam>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static TResult OnFail<TResult>(this TResult input, Action onFailAction)
        where TResult : IConclusion
    {
        if (input.IsFailed)
            onFailAction();

        return input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TResult">Type of conclusion</typeparam>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static TResult OnFail<TResult>(this TResult input, Action<IEnumerable<IError>> onFailAction)
        where TResult : IConclusion
    {
        if (input.IsFailed)
            onFailAction(input.Errors);

        return input;
    }
}