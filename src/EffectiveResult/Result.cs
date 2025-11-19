using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using EffectiveResult.Abstractions;
using EffectiveResult.Exceptions;
using EffectiveResult.Json;

namespace EffectiveResult;

/// <summary>
/// An implementation of the result monad pattern for an alternative way of handling errors.
/// </summary>
[JsonConverter(typeof(ResultJsonConverter))]
public sealed partial class Result : IConclusion, IEquatable<Result>
{
    private readonly ResultError[] _errors = [];

    /// <inheritdoc />
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyCollection<ResultError> Errors => _errors;

    internal Result()
    { }

    internal Result(ResultError error) => _errors = [error];

    internal Result(ResultError[] errors, bool isFailed = true)
    {
        _errors = errors;

        if (isFailed && _errors.Length == 0)
        {
            throw new InvalidResultOperationException("Can't create failed result without errors");
        }
    }

    /// <summary>
    /// Copy constructor for object cloning
    /// </summary>
    public Result(Result other) => _errors = other._errors;

    /// <summary>
    /// Provide conversion to <see cref="Result{TNewValue}"/> with same reasons
    /// </summary>
    /// <value>New value of result, can be null only when result is false</value>
    /// <returns>Result with provided value, only if source is success</returns>
    public Result<TNewValue> ToResult<TNewValue>(TNewValue? value = default) =>
        IsSuccess
            ? new Result<TNewValue>(value)
            : new Result<TNewValue>(_errors);

    /// <summary>
    /// Provide conversion to <see cref="Result{TNewValue}"/> with same reasons
    /// </summary>
    /// <value>New value of result, can be null only when result is false</value>
    /// <returns>Result with provided value, only if source is success</returns>
    /// <exception cref="ArgumentNullOnSuccessException">Can be thrown, if result is success and not provided new value</exception>
    public Result<TNewValue> ToResult<TNewValue>(Func<TNewValue> valueFactory) =>
        IsSuccess
            ? new Result<TNewValue>(valueFactory())
            : new Result<TNewValue>(_errors);

    /// Convert to failed result
    public static implicit operator Result(ResultError error) => Result.Fail(error);

    /// <summary>
    /// Provide method for fluent deconstruct type and use with syntactic sugar
    /// </summary>
    /// <param name="isSuccess">Status of result</param>
    /// <param name="errors">Errors on fail or empty collection on success</param>
    public void Deconstruct(out bool isSuccess, out IReadOnlyCollection<ResultError> errors)
    {
        isSuccess = IsSuccess;
        errors = _errors;
    }

    /// <summary>
    /// Convert to human-readable representation
    /// </summary>
    /// <returns>Human-readable string based on result state</returns>
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        const int predefinedSuccessSize = 26;
        var builder = new StringBuilder(predefinedSuccessSize);

        builder.Append("Result { State = ");
        builder.Append(IsSuccess ? "Success" : "Failed");

        if (IsFailed)
        {
            builder.Append(", Errors = [ ");
            builder.AppendJoin<ResultError>("; ", _errors);
            builder.Append(" ]");
        }

        builder.Append(" }");
        return builder.ToString();
    }

    /// <inheritdoc/>
    public bool Equals(Result? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return (obj1: this, obj2: other) switch
        {
            { obj1.IsSuccess: true, obj2.IsSuccess: true } => true,
            { obj1.IsFailed: true, obj2.IsFailed: true } => _errors.SequenceEqual(other._errors),
            _ => false
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Result);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_errors);

    /// Compare two results by equality comparing
    public static bool operator ==(Result? left, Result? right) => Equals(left, right);

    /// Compare two results by equality comparing
    public static bool operator !=(Result? left, Result? right) => Equals(left, right) is false;
}
