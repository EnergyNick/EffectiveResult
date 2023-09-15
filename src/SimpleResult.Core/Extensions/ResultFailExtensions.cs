using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ResultFailExtensions
{
    /// <summary>
    /// Call action only if <see cref="result"/> is failed
    /// </summary>
    public static TResult OnFail<TResult>(this TResult result, Action onFailAction)
        where TResult : IConclusion
    {
        if (result.IsFailed)
            onFailAction();

        return result;
    }

    /// <summary>
    /// Call action only if <see cref="result"/> is failed
    /// </summary>
    public static TResult OnFail<TResult>(this TResult result, Action<IEnumerable<IError>> onFailAction)
        where TResult : IConclusion
    {
        if (result.IsFailed)
            onFailAction(result.Errors);

        return result;
    }
}