using System.Collections.Immutable;
using SimpleResult.Core;

namespace SimpleResult;

/// <summary>
/// Base abstraction of all errors from SimpleResult library.
/// Provide useful methods and constructors for errors implementation
/// and default implementation of <see cref="IError"/> interface.
/// </summary>
public abstract record BaseError : IError
{
    private readonly ImmutableArray<IError> _causedErrors = ImmutableArray<IError>.Empty;

    /// <inheritdoc />
    public string Message { get; init; }

    /// <inheritdoc />
    public IReadOnlyCollection<IError> CausedErrors => _causedErrors;

    protected BaseError(string message) => Message = message;

    protected BaseError(string message, IError causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    protected BaseError(string message, params IError[] causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    protected BaseError(string message, IEnumerable<IError> causedBy) : this(message) =>
        _causedErrors = causedBy.ToImmutableArray();
}