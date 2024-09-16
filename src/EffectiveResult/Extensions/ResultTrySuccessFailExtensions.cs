using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ResultTrySuccessFailExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result OnSuccessTry(this Result input, Action continuation)
    {
        try
        {
            return input.OnSuccess(continuation);
        }
        catch (Exception e)
        {
            return new Result(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>
    /// Result from <paramref name="input"/> or copy with exception error from <paramref name="continuation"/>
    /// </returns>
    public static Result<TValue> OnSuccessTry<TValue>(this Result<TValue> input, Action<TValue> continuation)
    {
        try
        {
            return input.OnSuccess(continuation);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>

    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result OnFailTry(this Result input, Action onFailAction)
    {
        try
        {
            return input.OnFail(onFailAction);
        }
        catch (Exception e)
        {
            return new Result(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>

    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result<TValue> OnFailTry<TValue>(this Result<TValue> input, Action onFailAction)
    {
        try
        {
            return input.OnFail(onFailAction);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>

    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result OnFailTry(this Result input,
        Action<IEnumerable<IError>> onFailAction)
    {
        try
        {
            return input.OnFail(onFailAction);
        }
        catch (Exception e)
        {
            return new Result(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Call action only if <see cref="input"/> is failed
    /// </summary>
    /// <param name="input">Source of conclusion</param>
    /// <param name="onFailAction">Action for invoke on fail</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>

    /// <returns>Conclusion from <paramref name="input"/></returns>
    public static Result<TValue> OnFailTry<TValue>(this Result<TValue> input,
        Action<IEnumerable<IError>> onFailAction)
    {
        try
        {
            return input.OnFail(onFailAction);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }
}