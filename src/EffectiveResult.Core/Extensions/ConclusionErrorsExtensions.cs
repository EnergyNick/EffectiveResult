using System.Diagnostics.CodeAnalysis;
using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ConclusionErrorsExtensions
{
    /// <summary>
    /// Check, if in array of <see cref="IError"/> contains error of <see cref="TError"/> type.
    /// </summary>
    /// <param name="errors">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    /// <returns>True, if exists in enumerable</returns>
    public static bool HasErrorsOfType<TError>(this IEnumerable<IError> errors, Predicate<TError>? predicate = null)
        where TError : IError
    {
        var enumeratedReasons = errors as ICollection<IError> ?? errors.ToArray();

        return enumeratedReasons.Any(reason =>
            reason is TError reasonOfType
            && (predicate is null || predicate(reasonOfType)));
    }

    /// <summary>
    /// Check, if in array of <see cref="IError"/> contains error of <see cref="TError"/> type and in caused errors.
    /// </summary>
    /// <param name="errors">Source of errors</param>
    /// <param name="predicate">Additional error predicate</param>
    /// <typeparam name="TError">Type of error</typeparam>
    /// <returns>True, if exists in enumerable</returns>
    public static bool HasErrorsOfTypeRecursively<TError>(this IEnumerable<IError> errors, Predicate<TError>? predicate = null)
        where TError : IError
    {
        var enumeratedReasons = errors as ICollection<IError> ?? errors.ToArray();

        var anyErrors = enumeratedReasons.Any(reason =>
            reason is TError reasonOfType
            && (predicate is null || predicate(reasonOfType)));

        return anyErrors || enumeratedReasons.Any(error => HasErrorsOfTypeRecursively(error.CausedErrors, predicate));
    }

    /// <summary>
    /// Trying to get matching exception from conclusion.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="exception">Provide first exception match, if return true</param>
    /// <param name="filter">Filter for matching error</param>
    /// <returns>True, if conclusion contains matching error</returns>
    public static bool TryGetException(this IConclusion conclusion,
        [NotNullWhen(true)] out Exception? exception,
        Predicate<Exception>? filter = null)
    {
        return TryGetException<Exception>(conclusion, out exception, filter);
    }

    /// <summary>
    /// Trying to get matching exception from conclusion.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="exception">Provide first exception match, if return true</param>
    /// <param name="filter">Filter for matching error</param>
    /// <typeparam name="TException">Type of matching exception</typeparam>
    /// <returns>True, if conclusion contains matching error</returns>
    public static bool TryGetException<TException>(this IConclusion conclusion,
        [NotNullWhen(true)] out TException? exception,
        Predicate<TException>? filter = null)
        where TException : Exception
    {
        var error = conclusion.Errors
            .OfType<IExceptionalError>()
            .FirstOrDefault(e => e.Exception is TException ex && (filter?.Invoke(ex) ?? true));
        exception = error?.Exception as TException;
        return error != null;
    }

    /// <summary>
    /// Get exceptions from conclusion with filtering.
    /// </summary>
    /// <param name="conclusion">Source of errors</param>
    /// <param name="filter">Filter for matching error</param>
    /// <returns>Collections of matching exceptions from conclusion</returns>
    public static IEnumerable<Exception> GetExceptions(this IConclusion conclusion,
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
    public static IEnumerable<TException> GetExceptions<TException>(this IConclusion conclusion,
        Predicate<TException>? filter = null)
        where TException : Exception
    {
        return conclusion.Errors
            .OfType<IExceptionalError>()
            .Where(e => e.Exception is TException ex && (filter?.Invoke(ex) ?? true))
            .Select(x => (TException)x.Exception);
    }
}