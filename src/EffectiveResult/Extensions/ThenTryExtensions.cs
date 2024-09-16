using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ResultsThenTryExtensions
{
    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input, Func<TOutput> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result<TOutput>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result ThenTry(this Result input, Func<Result> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input, Func<Result<TOutput>> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result<TOutput>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(
        this Result<TInput> input,
        Func<TInput, TOutput> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result<TOutput>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result ThenTry<TInput>(
        this Result<TInput> input,
        Func<TInput, Result> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>\
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            return new Result<TOutput>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(
        this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, TValue> continuation)
    {
        try
        {
            return input.ThenOnFail(continuation);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(
        this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, Result<TValue>> continuation)
    {
        try
        {
            return input.ThenOnFail(continuation);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(
        this Result<TValue> input,
        Func<TValue> continuation)
    {
        try
        {
            return input.ThenOnFail(continuation);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(
        this Result<TValue> input,
        Func<Result<TValue>> continuation)
    {
        try
        {
            return input.ThenOnFail(continuation);
        }
        catch (Exception e)
        {
            return new Result<TValue>(input.Errors.Append(new ExceptionalError(e)));
        }
    }
}