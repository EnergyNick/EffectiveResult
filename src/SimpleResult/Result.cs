using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using SimpleResult.Core;
using SimpleResult.Exceptions;

namespace SimpleResult;

public sealed partial record Result : IConclusion
{
    private readonly ImmutableArray<IError> _errors = ImmutableArray<IError>.Empty;

    /// <inheritdoc />
    public bool IsSuccess => _errors.Length == 0;

    /// <inheritdoc />
    public bool IsFailed => _errors.Length != 0;

    /// <inheritdoc />
    public IReadOnlyCollection<IError> Errors => _errors;

    internal Result()
    { }

    internal Result(IError error) => _errors = ImmutableArray.Create(error);

    internal Result(IEnumerable<IError> errors, bool isFailed = true)
    {
        _errors = errors is IError[] arrayErrors
            ? ImmutableArray.Create(arrayErrors)
            : errors.ToImmutableArray();

        if (isFailed && _errors.Length == 0)
            throw new InvalidResultOperationException("Can't create failed result without errors");
    }

    public Result(Result other) => _errors = other._errors;

    /// <summary>
    /// Provide conversion to <see cref="Result{TNewValue}"/> with same reasons
    /// </summary>
    /// <value>New value of result, can be null only when result is false</value>
    /// <returns>Result with provided value, only if source is success</returns>
    public Result<TNewValue> ToResult<TNewValue>(TNewValue value)
    {
        if (IsSuccess && value is null)
            throw new ArgumentNullOnSuccessException(nameof(value));

        return IsSuccess
            ? new Result<TNewValue>(value!)
            : new Result<TNewValue>(_errors);
    }

    /// <summary>
    /// Provide conversion to <see cref="Result{TNewValue}"/> with same reasons
    /// </summary>
    /// <value>New value of result, can be null only when result is false</value>
    /// <returns>Result with provided value, only if source is success</returns>
    /// <exception cref="ArgumentNullOnSuccessException">Can be thrown, if result is success and not provided new value</exception>
    public Result<TNewValue> ToResult<TNewValue>(Func<TNewValue>? valueFactory = null)
    {
        if (IsSuccess && valueFactory is null)
            throw new ArgumentNullOnSuccessException(nameof(valueFactory));

        return IsSuccess
            ? new Result<TNewValue>(valueFactory!())
            : new Result<TNewValue>(_errors);
    }

    public static implicit operator Result(Error error) => Result.Fail(error);

    [ExcludeFromCodeCoverage]
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("IsSuccess = ");
        builder.Append(IsSuccess ? "true" : "false");
        if (IsFailed)
        {
            builder.Append(", Errors = [ ");
            builder.Append(string.Join("; ", _errors));
            builder.Append(" ]");
        }
        return true;
    }

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
}