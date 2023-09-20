using SimpleResult.Settings;

namespace SimpleResult.Extensions;

public static partial class ResultsThenExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result.
    /// On exception in "<paramref name="continuation"/>" catch and return failed result.
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TValue> ThenTry<TValue>(this Result<TValue> input, Action<TValue> continuation,
            Func<Exception, Error>? catchHandler = null)
    {
        try
        {
            return input.Then(continuation);
        }
        catch (Exception e)
        {
            catchHandler ??= ResultSettings.Current.TryCatchHandler;

            return new Result<TValue>(input.Errors.Append(catchHandler(e)));
        }
    }

    /// <summary>
    /// Provide chaining method for action on success result.
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
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result ThenTry(this Result input, Func<Result> continuation,
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
    /// <param name="input">Source result</param>
    /// <param name="continuation">Function for invoke on success</param>
    /// <typeparam name="TOutput">Type of "<paramref name="continuation"/>" result</typeparam>
    /// <param name="catchHandler">
    /// Transform exceptions to errors,
    /// on default use <see cref="ResultSettings"/>.<see cref="ResultSettings.Current"/>
    /// </param>
    /// <returns>Result of <paramref name="continuation"/> or errors from <paramref name="input"/></returns>
    public static Result<TOutput> ThenTry<TOutput>(this Result input, Func<Result<TOutput>> continuation,
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