using SimpleResult.Core;
using SimpleResult.Settings;

namespace SimpleResult.Extensions;

public static partial class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input, Func<TOutput> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result<TOutput>(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result ThenTry(this Result input, Func<Result> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input, Func<Result<TOutput>> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result<TOutput>(input.Errors.Append(catchHandler(e)));
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
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, TOutput> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result<TOutput>(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result ThenTry<TInput>(this Result<TInput> input,
        Func<TInput, Result> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result(input.Errors.Append(catchHandler(e)));
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
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result<TOutput>(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, TValue> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        return input.IsFailed
            ? continuation(input.Errors).MakeResult()
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, Result<TValue>> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        return input.IsFailed
            ? continuation(input.Errors)
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(this Result<TValue> input,
        Func<TValue> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        return input.IsFailed
            ? continuation().MakeResult()
            : input;
    }

    /// <summary>
    /// Provide chaining method for next operation on failed result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <see cref="input"/> or result from <paramref name="continuation"/> </returns>
    public static Result<TValue> ThenTryOnFail<TValue>(this Result<TValue> input,
        Func<Result<TValue>> continuation,
        Func<Exception, IError>? catchHandler = null)
    {
        return input.IsFailed
            ? continuation()
            : input;
    }
}