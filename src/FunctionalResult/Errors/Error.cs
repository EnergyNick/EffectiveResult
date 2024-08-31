using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FunctionalResult.Abstractions;

namespace FunctionalResult;

/// <summary>
/// Base type of all errors from FunctionalResult library.
/// Provide useful methods and constructors for errors implementation
/// and default implementation of <see cref="IError"/> interface.
/// <remarks> Used also for implicit cast of error to <see cref="Result"/> and <see cref="Result{TValue}"/>.</remarks>
/// </summary>
public record Error : IError
{
    private readonly ImmutableArray<IError> _causedErrors = ImmutableArray<IError>.Empty;

    /// <inheritdoc />
    public string Message { get; init; }

    /// <inheritdoc />
    public IReadOnlyCollection<IError> CausedErrors => _causedErrors;

    public Error(string message) => Message = message;

    public Error(string message, IError causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    public Error(string message, params IError[] causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    public Error(string message, IEnumerable<IError> causedBy) : this(message) =>
        _causedErrors = causedBy.ToImmutableArray();

    [ExcludeFromCodeCoverage]
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Message = ");
        builder.Append(Message);
        if (_causedErrors.Any())
        {
            builder.Append(", CausedErrors = [ ");
            builder.Append(string.Join("; ", _causedErrors));
            builder.Append(" ]");
        }
        return true;
    }
}