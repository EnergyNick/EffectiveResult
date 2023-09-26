using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ErrorMappingExtensions
{
    /// <summary>
    /// Provide error mapping on failed result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="errorMapper">Function for invoke on fail</param>
    /// <returns>Result from <see cref="input"/> on success or result with mapped errors</returns>
    public static Result MapErrorsOnFailed(this Result input,
        Func<IReadOnlyCollection<IError>, IEnumerable<IError>> errorMapper)
    {
        return input.IsFailed
            ? new Result(errorMapper(input.Errors))
            : input;
    }

    /// <summary>
    /// Provide error mapping on failed result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="errorMapper">Function for invoke on fail</param>
    /// <typeparam name="TValue">Type of "<paramref name="input"/>" result</typeparam>
    /// <returns>Result from <see cref="input"/> on success or result with mapped errors</returns>
    public static Result<TValue> MapErrorsOnFailed<TValue>(this Result<TValue> input,
        Func<IReadOnlyCollection<IError>, IEnumerable<IError>> errorMapper)
    {
        return input.IsFailed
            ? new Result<TValue>(errorMapper(input.Errors))
            : input;
    }
}