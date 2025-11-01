using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ConclusionSuccessFailExtensions
{
    /// <summary>
    /// Provide chaining final method for action of result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onSuccessAction">Action for invoke on success</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    public static void Match<TConclusion>(this TConclusion input, Action onSuccessAction, Action onFailAction)
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
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static TConclusion OnSuccess<TConclusion>(this TConclusion input, Action continuation)
        where TConclusion : IConclusion
    {
        if (input.IsSuccess)
        {
            continuation();
        }

        return input;
    }

    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static TConclusion OnSuccess<TConclusion, TValue>(this TConclusion input, Action<TValue> continuation)
        where TConclusion : IConclusion, IValueStorage<TValue>
    {
        if (input.IsSuccess)
        {
            continuation(input.ValueOrDefault!);
        }

        return input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static TConclusion OnFail<TConclusion>(this TConclusion input, Action onFailAction)
        where TConclusion : IConclusion
    {
        if (input.IsFailed)
        {
            onFailAction();
        }

        return input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TConclusion">Type of conclusion</typeparam>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static TConclusion OnFail<TConclusion>(this TConclusion input, Action<IEnumerable<Error>> onFailAction)
        where TConclusion : class, IConclusion
    {
        if (input.IsFailed)
        {
            onFailAction(input.Errors);
        }

        return input;
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed and contains exception with type <see cref="TException"/>
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TException">Type of searching error</typeparam>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static IConclusion OnFailWithException<TException>(this IConclusion input,
        Action<ExceptionalError> onFailAction)
        where TException : Exception
    {
        if (input.IsFailed)
        {
            var exceptionalError = input.Errors
                .OfType<ExceptionalError>()
                .FirstOrDefault(x => x.Exception is TException);

            if (exceptionalError is not null)
            {
                onFailAction(exceptionalError);
            }
        }

        return input;
    }
}
