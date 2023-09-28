using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using SimpleResult.Core;
using SimpleResult.Exceptions;

namespace SimpleResult;

public record Result<TValue> : IConclusion
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
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
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

    public static implicit operator Result<TValue>(in TValue value) => new(value);

    public static implicit operator Result<TValue>(BaseError error) => Result.Fail<TValue>(error);

    [ExcludeFromCodeCoverage]
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("IsSuccess = ");
        if (IsSuccess)
        {
            builder.Append("true, Value = ");
            builder.Append(_value);
        }
        else
        {
            builder.Append("false, Errors = ");
            builder.Append(_errors);
        }
        return true;
    }
}