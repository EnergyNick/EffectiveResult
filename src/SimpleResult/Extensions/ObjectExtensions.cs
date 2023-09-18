using System.Runtime.CompilerServices;
using SimpleResult.Core;

namespace SimpleResult.Extensions;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeResult<TValue>(this TValue value) => Result.Ok(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result MakeFailedResult(this IError error) =>
        Result.Fail(error);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeFailedResult<TValue>(this IError error) =>
        Result.Fail<TValue>(error);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result MakeFailedResult(this Exception exception)=>
        Result.Fail(exception);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeFailedResult<TValue>(this Exception exception)=>
        Result.Fail<TValue>(exception);
}