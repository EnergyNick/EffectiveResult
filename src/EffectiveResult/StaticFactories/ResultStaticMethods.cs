using System.Collections.Immutable;
using EffectiveResult.Abstractions;

namespace EffectiveResult;

public partial record Result
{
    private static readonly Result SuccessResult = new();

    /// <summary>
    /// Get empty result with success status
    /// </summary>
    public static Result Ok() => SuccessResult;

    /// <summary>
    /// Creates a failed result with the given error
    /// </summary>
    public static Result Fail(IError error) => new(error);

    /// <summary>
    /// Creates a failed result with the given errors
    /// </summary>
    public static Result Fail(IEnumerable<IError> errors) => new(errors);

    /// <summary>
    /// Creates a failed result with the given error message.
    /// Message will be transformed to <see cref="IError"/>
    /// </summary>
    public static Result Fail(string errorMessage) => Fail(new Error(errorMessage));

    /// <summary>
    /// Creates a failed result with the given exception.
    /// Message will be transformed to <see cref="ExceptionalError"/>
    /// </summary>
    public static Result Fail(Exception exception) => Fail(new ExceptionalError(exception));

    /// <summary>
    /// Creates a success result with the given value
    /// </summary>
    public static Result<TValue> Ok<TValue>(in TValue value) => new(value);

    /// <summary>
    /// Creates a failed result with the given error
    /// </summary>
    public static Result<TValue> Fail<TValue>(IError error) => new(error);

    /// <summary>
    /// Creates a failed result with the given errors
    /// </summary>
    public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors) => new(errors);

    /// <summary>
    /// Creates a failed result with the given error message.
    /// Message will be transformed to <see cref="Error"/>
    /// </summary>
    public static Result<TValue> Fail<TValue>(string errorMessage) => Fail<TValue>(new Error(errorMessage));

    /// <summary>
    /// Creates a failed result with the given exception.
    /// Message will be transformed to <see cref="ExceptionalError"/>
    /// </summary>
    public static Result<TValue> Fail<TValue>(Exception exception) => Fail<TValue>(new ExceptionalError(exception));

    /// <summary>
    /// Executes the action and catch all exceptions, If they will be thrown within the action.
    /// </summary>
    public static Result Try(Action action)
    {
        try
        {
            action();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    /// <summary>
    /// Executes the async action and catch all exceptions, If they will be thrown within the action.
    /// </summary>
    public static async Task<Result> TryAsync(Func<Task> action)
    {
        try
        {
            await action();
            return Ok();
        }
        catch (Exception e)
        {
            return Fail(e);
        }
    }

    /// <summary>
    /// Executes the action with return value and catch all exceptions, If they will be thrown within the action.
    /// </summary>
    public static Result<T> Try<T>(Func<T> action)
    {
        try
        {
            return Ok(action());
        }
        catch (Exception e)
        {
            return Fail<T>(e);
        }
    }

    /// <summary>
    /// Executes the async action with return value and catch all exceptions, If they will be thrown within the action.
    /// </summary>
    public static async Task<Result<T>> TryAsync<T>(Func<Task<T>> action)
    {
        try
        {
            return Ok(await action());
        }
        catch (Exception e)
        {
            return Fail<T>(e);
        }
    }

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result OkIf(bool condition, IError error) => condition ? Ok() : Fail(error);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result OkIf(bool condition, string error) => condition ? Ok() : Fail(error);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result OkIf(bool condition, Exception exception) => condition ? Ok() : Fail(exception);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result FailIf(bool condition, IError error) => condition ? Fail(error) : Ok();

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result FailIf(bool condition, string error) => condition ? Fail(error) : Ok();

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result FailIf(bool condition, Exception exception) => condition ? Fail(exception) : Ok();

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> OkIf<TValue>(bool condition, IError error, TValue valueIfSuccess) =>
        condition ? Ok(valueIfSuccess) : Fail<TValue>(error);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> OkIf<TValue>(bool condition, string error, TValue valueIfSuccess) =>
        condition ? Ok(valueIfSuccess) : Fail<TValue>(error);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> OkIf<TValue>(bool condition, Exception exception, TValue valueIfSuccess) =>
        condition ? Ok(valueIfSuccess) : Fail<TValue>(exception);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> FailIf<TValue>(bool condition, IError error, TValue valueIfSuccess) =>
        condition ? Fail<TValue>(error) : Ok(valueIfSuccess);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> FailIf<TValue>(bool condition, string error, TValue valueIfSuccess) =>
        condition ? Fail<TValue>(error) : Ok(valueIfSuccess);

    /// <summary>
    /// Create result with status depending on condition
    /// </summary>
    public static Result<TValue> FailIf<TValue>(bool condition, Exception exception, TValue valueIfSuccess) =>
        condition ? Fail<TValue>(exception) : Ok(valueIfSuccess);

    /// <summary>
    /// Provide method for combining results to single result
    /// </summary>
    /// <param name="results">Input results to merge</param>
    /// <returns>Result with combined status of <see cref="results"/></returns>
    public static Result Combine(IEnumerable<Result> results)
    {
        var failed = results
            .Where(x => x.IsFailed)
            .SelectMany(x => x.Errors)
            .ToImmutableArray();

        return failed.Length == 0
            ? Ok()
            : Fail(failed);
    }

    /// <summary>
    /// Provide method for combining results to single result
    /// </summary>
    /// <param name="results">Input results to merge</param>
    /// <returns>Result with combined status of <see cref="results"/></returns>
    public static Result Combine(params Result[] results) => Combine(results.AsEnumerable());

    /// <summary>
    /// Provide method for combining results to single result with values.
    /// </summary>
    /// <param name="results">Input results to merge</param>
    /// <returns>Result with combined status of <see cref="results"/></returns>
    public static Result<IEnumerable<TValue>> Combine<TValue>(IEnumerable<Result<TValue>> results)
    {
        var enumerated = results as ICollection<Result<TValue>> ?? results.ToArray();
        var failed = enumerated.Any(x => x.IsFailed);

        return failed
            ? Fail<IEnumerable<TValue>>(enumerated.SelectMany(x => x.Errors))
            : Ok(enumerated.Select(x => x.Value));
    }

    /// <summary>
    /// Provide method for combining results to single result with values.
    /// </summary>
    /// <param name="results">Input results to merge</param>
    /// <returns>Result with combined status of <see cref="results"/></returns>
    public static Result<IEnumerable<TValue>> Combine<TValue>(params Result<TValue>[] results) =>
        Combine(results.AsEnumerable());
}