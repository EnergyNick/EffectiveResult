using System.Diagnostics.CodeAnalysis;
using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ConclusionErrorsExtensions
{
    /// <summary>
    /// Check, if in array of <see cref="ResultError"/> contains error of <see cref="TError"/> type.
    /// </summary>
    /// <param name="errors">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    /// <returns>True, if exists in enumerable</returns>
    public static bool HasErrorsOfType<TError>(
        this IEnumerable<ResultError> errors,
        Predicate<TError>? predicate = null)
        where TError : ResultError
    {
        var enumeratedReasons = errors as ICollection<ResultError> ?? [.. errors];

        return enumeratedReasons.Any(reason =>
            reason is TError reasonOfType
            && (predicate is null || predicate(reasonOfType)));
    }

    /// <summary>
    /// Check, if in array of <see cref="ResultError"/> contains error of <see cref="TError"/> type and in caused errors.
    /// </summary>
    /// <param name="errors">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    /// <returns>True, if exists in enumerable</returns>
    public static bool HasErrorsOfTypeRecursively<TError>(
        this IEnumerable<ResultError> errors,
        Predicate<TError>? predicate = null)
        where TError : ResultError
    {
        var enumeratedReasons = errors as ICollection<ResultError> ?? [.. errors];

        var anyErrors = enumeratedReasons.Any(reason =>
            reason is TError reasonOfType
            && (predicate is null || predicate(reasonOfType)));

        return anyErrors || enumeratedReasons.Any(error =>
            error.CausedErrors is { Count: > 0 } && error.CausedErrors.HasErrorsOfTypeRecursively(predicate));
    }

    /// <summary>
    /// Get collection of <see cref="ResultError"/> contains error of <see cref="TError"/> type.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    public static IEnumerable<TError> GetErrorsOfType<TError>(
        this IConclusion conclusion,
        Predicate<TError>? predicate = null)
        where TError : ResultError
    {
        if (conclusion.IsSuccess)
        {
            return [];
        }

        var typedErrors = conclusion.Errors.OfType<TError>();
        return predicate is not null
            ? typedErrors.Where(e => predicate(e))
            : typedErrors;
    }

    /// <summary>
    /// Get collection of <see cref="ResultError"/> contains error of <see cref="TError"/> type and in caused errors.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    public static IEnumerable<TError> GetErrorsOfTypeRecursively<TError>(
        this IConclusion conclusion,
        Predicate<TError>? predicate = null)
        where TError : ResultError
    {
        if (conclusion.IsSuccess)
        {
            return [];
        }

        var typedErrors = conclusion.Errors
            .Concat(conclusion.Errors.SelectMany(e => e.CausedErrors))
            .OfType<TError>();
        return predicate is not null
            ? typedErrors.Where(e => predicate(e))
            : typedErrors;
    }

    /// <summary>
    /// Trying to get first matching exception from conclusion.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="exception">Provide first exception match, if return true</param>
    /// <param name="filter">Filter for matching exception</param>
    /// <returns>True, if conclusion contains matching exception</returns>
    public static bool TryGetException(
        this IConclusion conclusion,
        [NotNullWhen(true)] out Exception? exception,
        Predicate<Exception>? filter = null)
    {
        return TryGetException<Exception>(conclusion, out exception, filter);
    }

    /// <summary>
    /// Trying to get first matching exception from conclusion.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="exception">Provide first exception match, if return true</param>
    /// <param name="filter">Filter for matching exception</param>
    /// <typeparam name="TException">Type of matching exception</typeparam>
    /// <returns>True, if conclusion contains matching exception</returns>
    public static bool TryGetException<TException>(
        this IConclusion conclusion,
        [NotNullWhen(true)] out TException? exception,
        Predicate<TException>? filter = null)
        where TException : Exception
    {
        var error = conclusion.Errors
            .FirstOrDefault(e => e.Exception is TException ex && (filter?.Invoke(ex) ?? true));

        exception = error?.Exception as TException;
        return error is not null;
    }

    /// <summary>
    /// Get exceptions from conclusion with filtering.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="filter">Filter for matching error</param>
    /// <returns>Collections of matching exceptions from conclusion</returns>
    public static IEnumerable<Exception> GetExceptions(
        this IConclusion conclusion,
        Predicate<Exception>? filter = null)
    {
        return GetExceptions<Exception>(conclusion, filter);
    }

    /// <summary>
    /// Get exceptions with specific types from conclusion with filtering.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="filter">Filter for matching error</param>
    /// <typeparam name="TException">Type of exception</typeparam>
    /// <returns>Collections of matching exceptions from conclusion</returns>
    public static IEnumerable<TException> GetExceptions<TException>(
        this IConclusion conclusion,
        Predicate<TException>? filter = null)
        where TException : Exception
    {
        return conclusion.Errors
            .Where(e => e.Exception is TException ex && (filter?.Invoke(ex) ?? true))
            .Select(x => (TException)x.Exception!);
    }
}
