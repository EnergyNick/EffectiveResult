using System.Runtime.CompilerServices;
using EffectiveResult.Abstractions;

namespace EffectiveResult.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Create successful result from object as storing value
    /// </summary>
    /// <typeparam name="TValue">Type of value</typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeResult<TValue>(this TValue value) =>
        Result.Ok(value);

    /// <summary>
    /// Create failed result from error
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result MakeFailedResult(this IError error) =>
        Result.Fail(error);

    /// <summary>
    /// Create failed typed result from error
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeFailedResult<TValue>(this IError error) =>
        Result.Fail<TValue>(error);

    /// <summary>
    /// Create failed result from exception as error
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result MakeFailedResult(this Exception exception) =>
        Result.Fail(exception);

    /// <summary>
    /// Create failed typed result from exception as error
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> MakeFailedResult<TValue>(this Exception exception) =>
        Result.Fail<TValue>(exception);
}