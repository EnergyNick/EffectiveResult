using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ObjectExtensions
{
    public static Result<TValue> MakeResult<TValue>(this TValue value) => Result.Ok(value);

    public static Result<TValue> MakeFailedResult<TValue>(this IError error) =>
        Result.Fail<TValue>(error);

    public static Result<TValue> MakeFailedResult<TValue>(this Exception exception)=>
        Result.Fail<TValue>(exception);
}