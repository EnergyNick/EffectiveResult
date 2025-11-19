using System.Diagnostics.CodeAnalysis;
using System.Text;
using EffectiveResult.Abstractions;
using EffectiveResult.Exceptions;

namespace EffectiveResult;

/// <summary>
/// An implementation of the result monad pattern for an alternative way of handling errors.
/// Can store value on success state.
/// </summary>
public sealed class Result<TValue>
    : IConclusion, IValueStorage<TValue>, IReferenceValueStorage<TValue>, IEquatable<Result<TValue>>
{
    private readonly ResultError[] _errors = [];
    private readonly TValue? _value;

    /// <inheritdoc />
    public TValue? ValueOrDefault => _value;

    /// <inheritdoc />
    public TValue Value => IsSuccess ? _value! : throw new OperationOnFailedResultException("Get value");

    /// <inheritdoc />
    public ref readonly TValue? ValueOrDefaultRef => ref _value;

    /// <inheritdoc />
    public ref readonly TValue ValueRef
    {
        get
        {
            if (IsFailed)
            {
                throw new OperationOnFailedResultException("Get value");
            }

            return ref _value!;
        }
    }

    /// <inheritdoc />
    [MemberNotNullWhen(true, nameof(Value), nameof(ValueOrDefault))]
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Value), nameof(ValueOrDefault))]
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyCollection<ResultError> Errors => _errors;

    internal Result(in TValue? value) => _value = value;

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
    /// Copy constructor for object cloning and also copy storing value, if exists
    /// </summary>
    public Result(Result<TValue> other)
    {
        if (other.IsSuccess)
        {
            _value = other._value;
        }

        _errors = other._errors;
    }

    /// <summary>
    /// Return value from result on success or value from <see cref="defaultValue"/> on failed.
    /// </summary>
    /// <param name="defaultValue">Factory of default value on failed status</param>
    /// <returns>Value based on result status</returns>
    public TValue GetValueOrDefault(TValue defaultValue) => IsSuccess ? _value! : defaultValue;

    /// <summary>
    /// Return value from result on success or value from <see cref="defaultValueFactory"/> on failed.
    /// </summary>
    /// <param name="defaultValueFactory">Factory of default value on failed status</param>
    /// <returns>Value based on result status</returns>
    public TValue GetValueOrDefault(Func<TValue> defaultValueFactory) => IsSuccess ? _value! : defaultValueFactory();

    /// <summary>
    /// TryGet patter realization, return value, if result is success
    /// </summary>
    /// <param name="value">Value or default</param>
    /// <returns>Is result success</returns>
    public bool TryGetValue([NotNullWhen(true)] out TValue? value)
    {
        value = ValueOrDefault;
        return IsSuccess;
    }

    /// <summary>
    /// Provide conversion to <see cref="Result"/> with same reasons
    /// </summary>
    /// <returns>Copy of current result with new value</returns>
    public Result ToResult() => new(_errors, IsFailed);

    /// <summary>
    /// Provide conversion to <see cref="Result{TNewValue}"/> with same reasons
    /// </summary>
    /// <value>New value of result, can be null only when result is false</value>
    /// <returns>Result with provided value, only if source is success</returns>
    public Result<TNewValue> ToResult<TNewValue>(TNewValue? valueIfSuccess = default) =>
        IsSuccess
            ? new Result<TNewValue>(valueIfSuccess)
            : new Result<TNewValue>(_errors);

    /// <summary>
    /// Provide conversion to <see cref="Result{TValue}"/> with value changing
    /// </summary>
    /// <param name="converter"></param>
    /// <returns>New result with converted value, if source is success</returns>
    /// <exception cref="ArgumentNullOnSuccessException">Can be thrown, if result is success and not provided converter</exception>
    public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> converter) =>
        IsSuccess
            ? new Result<TNewValue>(converter(_value!))
            : new Result<TNewValue>(_errors);

    /// Convert to success result
    public static implicit operator Result<TValue>(in TValue value) => new(value);

    /// Convert to failed result
    public static implicit operator Result<TValue>(ResultError error) => Result.Fail<TValue>(error);

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
    /// Provide method for fluent deconstruct type and use with syntactic sugar
    /// </summary>
    /// <param name="isSuccess">Status of result</param>
    /// <param name="valueOrDefault">Value on success or default value on fail</param>
    /// <param name="errors">Errors on fail or empty collection on success</param>
    public void Deconstruct(out bool isSuccess, out TValue? valueOrDefault, out IReadOnlyCollection<ResultError> errors)
    {
        isSuccess = IsSuccess;
        valueOrDefault = _value;
        errors = _errors;
    }

    /// <summary>
    /// Convert to human-readable representation
    /// </summary>
    /// <returns>Human-readable string based on result state</returns>
    [ExcludeFromCodeCoverage]
    public override string ToString()
    {
        var builder = new StringBuilder();

        builder.Append("Result { State = ");
        builder.Append(IsSuccess ? "Success, Value = '" : "Failed, Errors = [ ");

        if (IsSuccess)
        {
            builder.Append(_value is not null ? _value.ToString() : "null");
            builder.Append('\'');
        }
        else
        {
            builder.AppendJoin<ResultError>("; ", _errors);
            builder.Append(" ]");
        }

        builder.Append(" }");
        return builder.ToString();
    }

    /// <inheritdoc/>
    public bool Equals(Result<TValue>? other)
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
            { obj1.IsSuccess: true, obj2.IsSuccess: true } =>
                EqualityComparer<TValue?>.Default.Equals(_value, other._value),
            { obj1.IsFailed: true, obj2.IsFailed: true } =>
                _errors.SequenceEqual(other._errors),
            _ => false
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Result<TValue>);

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(_value, _errors);

    /// Compare two results by equality comparing
    public static bool operator ==(Result<TValue>? left, Result<TValue>? right) => Equals(left, right);

    /// Compare two results by equality comparing
    public static bool operator !=(Result<TValue>? left, Result<TValue>? right) => !Equals(left, right);
}
