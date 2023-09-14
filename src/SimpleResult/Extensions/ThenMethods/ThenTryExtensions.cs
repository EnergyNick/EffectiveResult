using SimpleResult.Settings;

namespace SimpleResult.Extensions;

public static partial class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result ThenTry(this Result input,
        Action continuation,
        Func<Exception, Error>? catchHandler = null)
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
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result<TOutput> input,
        Action<TOutput> continuation,
        Func<Exception, Error>? catchHandler = null)
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
    /// Provide chaining method for next function on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input,
        Func<TOutput> continuation,
        Func<Exception, Error>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return Result.Fail<TOutput>(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for next function on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input,
        Func<Result<TOutput>> continuation,
        Func<Exception, Error>? catchHandler = null)
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
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, Result<TOutput>> continuation,
        Func<Exception, Error>? catchHandler = null)
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
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source of success status</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <typeparam name="TInput">Type <paramref name="input"/> result</typeparam>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TInput, TOutput>(this Result<TInput> input,
        Func<TInput, TOutput> continuation,
        Func<Exception, Error>? catchHandler = null)
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
}