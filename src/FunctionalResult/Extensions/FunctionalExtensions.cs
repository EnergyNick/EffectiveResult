namespace FunctionalResult.Extensions;

public static class FunctionalExtensions
{
    /// <summary>
    /// Provide ability to merge result of result to flat result with combined status.
    /// </summary>
    /// <param name="result">Source with inner result</param>
    /// <returns>Result with merged status (Success only if all results are success)</returns>
    public static Result MergeInnerResult(this Result<Result> result) => result switch
    {
        { IsSuccess: true, ValueOrDefault.IsSuccess: true } => result.ValueOrDefault,
        { IsSuccess: true, ValueOrDefault.IsSuccess: false } =>
            Result.Fail(result.ValueOrDefault.Errors),
        _ => Result.Fail(result.Errors)
    };

    /// <summary>
    /// Provide ability to merge result of result to flat result with combined status.
    /// </summary>
    /// <param name="result">Source with inner result</param>
    /// <typeparam name="TValue">Type of inner result value</typeparam>
    /// <returns>Result with merged status (Success only if all results are success)</returns>
    public static Result<TValue> MergeInnerResult<TValue>(this Result<Result<TValue>> result) => result switch
    {
        { IsSuccess: true, ValueOrDefault.IsSuccess: true } => result.ValueOrDefault,
        { IsSuccess: true, ValueOrDefault.IsSuccess: false } =>
            Result.Fail<TValue>(result.ValueOrDefault.Errors),
        _ => Result.Fail<TValue>(result.Errors)
    };
}