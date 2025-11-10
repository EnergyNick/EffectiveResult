using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace EffectiveResult;

/// <summary>
/// Represents the base type of all error causes.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Only store data without any logic (only for debug printing)")]
public record ResultError
{
    private readonly IReadOnlyCollection<ResultError> _causedErrors;

    /// <summary>
    /// Gets a message that describes the current error.
    /// </summary>
    /// <returns>The error message that explains the reason, or an empty string ("").</returns>
    public string Message { get; init; }

    /// <summary>
    /// Caused exception from operation.
    /// </summary>
    public Exception? Exception { get; init; }

    /// <summary>
    /// Errors causing this error.
    /// </summary>
    public IReadOnlyCollection<ResultError> CausedErrors
    {
        get => _causedErrors;
        init => _causedErrors = value is not null ? [.. value] : [];
    }

    public ResultError(string message, Exception? exception = null, IEnumerable<ResultError>? causedErrors = null)
    {
        Message = message;
        Exception = exception;
        _causedErrors = causedErrors is not null ? [.. causedErrors] : [];
    }

    public ResultError(string message, IEnumerable<ResultError> causedErrors)
        : this(message, null, causedErrors)
    {
    }

    public ResultError(string message, params ResultError[] causedErrors)
        : this(message, null, causedErrors)
    {
    }

    public ResultError(Exception exception) : this(exception.Message, exception)
    {
    }

    public ResultError(Exception exception, IEnumerable<ResultError> causedErrors)
        : this(exception.Message, exception, causedErrors)
    {
    }

    public ResultError(Exception exception, params ResultError[] causedErrors)
        : this(exception.Message, exception, causedErrors)
    {
    }

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        if (Exception is not null)
        {
            if (string.Equals(Message, Exception.Message, StringComparison.InvariantCulture) is false)
            {
                builder.Append("Message = '");
                builder.Append(Message);
                builder.Append('\'');
            }

            builder.Append(", Exception = '");
            builder.Append(Exception);
        }
        else
        {
            builder.Append("Message = '");
            builder.Append(Message);
        }

        builder.Append('\'');

        if (CausedErrors.Count != 0)
        {
            builder.Append(", CausedErrors = [ ");
            builder.AppendJoin("; ", CausedErrors);
            builder.Append(" ]");
        }

        return true;
    }
}
