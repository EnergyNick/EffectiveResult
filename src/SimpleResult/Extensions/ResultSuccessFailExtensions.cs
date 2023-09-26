namespace SimpleResult.Extensions;

public static class ResultSuccessFailExtensions
{
    /// <summary>
    /// Provide chaining method for action on success result
    /// </summary>
    /// <param name="input">Source result</param>
    /// <param name="continuation">Action for invoke on success</param>
    /// <typeparam name="TValue">Type of result value on success</typeparam>
    /// <returns>Result from <paramref name="input"/></returns>
    public static Result<TValue> OnSuccess<TValue>(this Result<TValue> input, Action<TValue> continuation)
    {
        if (input.IsSuccess)
            continuation(input.ValueOrDefault!);
        return input;
    }
}