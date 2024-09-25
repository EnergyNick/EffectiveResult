using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using EffectiveResult.Abstractions;
using EffectiveResult.Exceptions;

namespace EffectiveResult;

public sealed class Result<TValue> : IConclusion, IValueStorage<TValue>, IEquatable<Result<TValue>>
{
    private readonly ImmutableArray<IError> _errors = ImmutableArray<IError>.Empty;
    private readonly TValue? _value;

    /// <summary>
    /// Return current result value (If result has failed status, will be returned default value)
    /// </summary>
    public ref readonly TValue? ValueOrDefault => ref _value;

    /// <summary>
    /// Return current result value (If result has failed status, an exception will be thrown)
    /// </summary>
    /// <exception cref="OperationOnFailedResultException">Thrown if result has failed status</exception>
    public ref readonly TValue Value
    {
        get
        {
            if (IsFailed)
                throw new OperationOnFailedResultException("Get value");

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
    public IReadOnlyCollection<IError> Errors => _errors;

    internal Result(in TValue value) => _value = value;

    internal Result(IError error) => _errors = ImmutableArray.Create(error);

    internal Result(IEnumerable<IError> errors, bool isFailed = true)
    {
        _errors = errors is IError[] arrayErrors
            ? ImmutableArray.Create(arrayErrors)
            : errors.ToImmutableArray();

        if (isFailed && _errors.Length == 0)
            throw new InvalidResultOperationException("Can't create failed result without errors");
    }

    /// <summary>
    /// Copy constructor for object cloning and also copy storing value, if exists
    /// </summary>
    public Result(Result<TValue> other)
    {
        if (other.IsSuccess)
            _value = other._value;

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
    /// Provide conversion to <see cref="Result"/> with same reasons
    /// </summary>
    /// <returns>Copy of current result with new value</returns>
    public Result ToResult() => new(_errors, IsFailed);

    /// <summary>
    /// Provide conversion to <see cref="Result{TValue}"/> with value changing
    /// </summary>
    /// <param name="converter"></param>
    /// <returns>New result with converted value, if source is success</returns>
    /// <exception cref="ArgumentNullOnSuccessException">Can be thrown, if result is success and not provided converter</exception>
    public Result<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue>? converter = null)
    {
        if (IsSuccess && converter is null)
            throw new ArgumentNullOnSuccessException(nameof(converter));

        return IsSuccess
            ? new Result<TNewValue>(converter!(_value!))
            : new Result<TNewValue>(_errors);
    }

    /// Convert to success result
    public static implicit operator Result<TValue>(in TValue value) => new(value);

    /// Convert to failed result
    public static implicit operator Result<TValue>(Error error) => Result.Fail<TValue>(error);

    /// <summary>
    /// Provide method for fluent deconstruct type and use with syntactic sugar
    /// </summary>
    /// <param name="isSuccess">Status of result</param>
    /// <param name="errors">Errors on fail or empty collection on success</param>
    public void Deconstruct(out bool isSuccess, out IReadOnlyCollection<IError> errors)
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
    public void Deconstruct(out bool isSuccess, out TValue? valueOrDefault, out IReadOnlyCollection<IError> errors)
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
            builder.AppendJoin("; ", _errors);
            builder.Append(" ]");
        }

        builder.Append(" }");
        return builder.ToString();
    }

    /// <inheritdoc/>
    public bool Equals(Result<TValue>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return (obj1: this, obj2: other) switch
        {
            { obj1.IsSuccess: true, obj2.IsSuccess: true } =>
                EqualityComparer<TValue?>.Default.Equals(_value, other._value),
            { obj1.IsFailed: true, obj2.IsFailed: true } =>
                _errors.Equals(other._errors),
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