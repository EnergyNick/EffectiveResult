using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EffectiveResult;

/// <summary>
/// Represents the base type of all error causes.
/// </summary>
public record Error
{
    private readonly ImmutableArray<Error> _causedErrors = [];

    /// <summary>
    /// Gets a message that describes the current error.
    /// </summary>
    /// <returns>The error message that explains the reason, or an empty string ("").</returns>
    public string Message { get; init; }

    /// <summary>
    /// Errors causing this error.
    /// </summary>
    public IReadOnlyCollection<Error> CausedErrors => _causedErrors;

    public Error(string message) => Message = message;

    public Error(string message, Error causedBy) : this(message) =>
        _causedErrors = [causedBy];

    public Error(string message, params Error[] causedBy) : this(message) =>
        _causedErrors = [.. causedBy];

    public Error(string message, IEnumerable<Error> causedBy) : this(message) =>
        _causedErrors = [.. causedBy];

    [ExcludeFromCodeCoverage]
    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Message = '");
        builder.Append(Message);
        builder.Append('\'');

        if (_causedErrors.Length != 0)
        {
            builder.Append(", CausedErrors = [ ");
            builder.AppendJoin("; ", _causedErrors);
            builder.Append(" ]");
        }
        return true;
    }
}
