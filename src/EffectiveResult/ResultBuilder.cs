using EffectiveResult.Abstractions;

namespace EffectiveResult;

/// <summary>
/// Mutable builder for result errors
/// </summary>
public sealed class ResultBuilder
{
    private readonly List<IError> _errors;

    internal ResultBuilder() => _errors = new List<IError>();

    internal ResultBuilder(int capacity) => _errors = new List<IError>(capacity);

    internal ResultBuilder(IEnumerable<IError> errors) => _errors = new List<IError>(errors);

    /// <summary>
    /// Add new error to builder state
    /// </summary>
    public ResultBuilder AppendError(IError error)
    {
        _errors.Add(error);
        return this;
    }

    /// <summary>
    /// Add new <see cref="Error"/> with message and add to builder state
    /// </summary>
    public ResultBuilder AppendError(string errorMessage)
    {
        _errors.Add(new Error(errorMessage));
        return this;
    }

    /// <summary>
    /// Add new <see cref="ExceptionalError"/> from exception and add to builder state
    /// </summary>
    public ResultBuilder AppendError(Exception exception)
    {
        _errors.Add(new ExceptionalError(exception));
        return this;
    }

    /// <summary>
    /// Add new errors to builder state
    /// </summary>
    public ResultBuilder AppendErrors(IEnumerable<IError> errors)
    {
        _errors.AddRange(errors);
        return this;
    }

    /// <summary>
    /// Add new errors to builder state
    /// </summary>
    public ResultBuilder AppendErrors(params IError[] errors)
    {
        _errors.AddRange(errors);
        return this;
    }

    /// <summary>
    /// Add errors from result to builder state
    /// </summary>
    public ResultBuilder AppendStateOf(Result source)
    {
        _errors.AddRange(source.Errors);
        return this;
    }

    /// <summary>
    /// Add errors from typed result to builder state
    /// </summary>
    public ResultBuilder AppendStateOf<TValue>(Result<TValue> source)
    {
        _errors.AddRange(source.Errors);
        return this;
    }

    /// <summary>
    /// Add errors from other builder to current builder state
    /// </summary>
    public ResultBuilder AppendStateOf(ResultBuilder other)
    {
        _errors.AddRange(other._errors);
        return this;
    }

    /// <summary>
    /// Create result object from current builder state
    /// </summary>
    public Result ToResult() => new(_errors);

    /// <summary>
    /// Create result object from current builder state
    /// </summary>
    /// <param name="valueIfSuccess">Value for result, if current builder state is success</param>
    /// <returns>Success or fail result based on result builder state</returns>
    public Result<TValue> ToResult<TValue>(in TValue valueIfSuccess) =>
        _errors.Any()
            ? new Result<TValue>(_errors)
            : new Result<TValue>(valueIfSuccess);

    /// <summary>
    /// Create result object from current builder state
    /// </summary>
    /// <param name="valueFactoryIfSuccess">Value factory for result, if current builder state is success</param>
    /// <returns>Success or fail result based on result builder state</returns>
    public Result<TValue> ToResult<TValue>(Func<TValue> valueFactoryIfSuccess) =>
        _errors.Any()
            ? new Result<TValue>(_errors)
            : new Result<TValue>(valueFactoryIfSuccess());

    /// <summary>
    /// Default way to create result builder
    /// </summary>
    /// <returns>New empty builder</returns>
    public static ResultBuilder Create() => new();

    /// <summary>
    /// Default way to create result builder
    /// </summary>
    /// <param name="capacity">Pre-defined size for error storage</param>
    /// <returns>New empty builder</returns>
    public static ResultBuilder Create(int capacity) => new(capacity);

    /// <summary>
    /// Create builder with initial errors
    /// </summary>
    /// <returns>New builder with added errors</returns>
    public static ResultBuilder Create(IEnumerable<IError> errors) => new(errors);

    /// <summary>
    /// Create builder with initial errors
    /// </summary>
    /// <returns>New builder with added errors</returns>
    public static ResultBuilder Create(params IError[] errors) => new(errors);
}