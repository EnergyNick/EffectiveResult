using System.Collections.Immutable;
using System.ComponentModel;
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
    public TValue? ValueOrDefault => _value;

    /// <summary>
    /// Return current result value (If result has failed status, an exception will be thrown)
    /// </summary>
    /// <exception cref="OperationOnFailedResultException">Thrown if result has failed status</exception>
    public TValue Value
    {
        get
        {
            if (IsFailed)
                throw new OperationOnFailedResultException("Get value");

            return _value!;
        }
    }

    /// <inheritdoc />
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyCollection<IError> Errors => _errors;

    internal Result(TValue value) => _value = value;

    internal Result(IError error) => _errors = ImmutableArray.Create(error);

    internal Result(params IError[] errors)
    {
        _errors = ImmutableArray.Create(errors);
        if (_errors.Length == 0)
            throw new InvalidResultOperationException("Can't create failed result without errors");
    }

    internal Result(IEnumerable<IError> errors)
    {
        _errors = errors.ToImmutableArray();
        if (_errors.Length == 0)
            throw new InvalidResultOperationException("Can't create failed result without errors");
    }

    public Result(Result<TValue> other)
    {
        if (other.IsSuccess)
            _value = other.ValueOrDefault;
        else
            _errors = other._errors;
    }

    /// <summary>
    /// Provide conversion to <see cref="Result"/> with same reasons
    /// </summary>
    /// <returns>Copy of current result with new value</returns>
    public Result ToResult() => new(_errors);

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
}