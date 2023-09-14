using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ResultFailExtensions
{
    /// <summary>
    /// Used to find the necessary error in result.
    /// </summary>
    /// <typeparam name="TError">Type of searching error</typeparam>
    public static bool HasError<TError>(this IConclusion conclusion, Predicate<TError>? predicate = null)
        where TError : IError
    {
        return conclusion.Errors.Any(x => x is TError error && (predicate?.Invoke(error) ?? true));
    }

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