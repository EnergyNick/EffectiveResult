using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using EffectiveResult.Json;

namespace EffectiveResult;

/// <summary>
/// Represents the base type of all error causes.
/// </summary>
[JsonConverter(typeof(ResultErrorJsonConverter))]
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

    [JsonConstructor]
    public ResultError(string message, Exception? exception, IEnumerable<ResultError>? causedErrors)
    {
        Message = message;
        Exception = exception;
        _causedErrors = causedErrors is not null ? [.. causedErrors] : [];
    }

    public ResultError(string message, Exception exception) : this(message, exception, null)
    {
    }

    public ResultError(string message, params ResultError[] causedErrors)
        : this(message, null, causedErrors)
    {
    }

    public ResultError(Exception exception) : this(exception.Message, exception, null)
    {
    }

    public ResultError(Exception exception, params ResultError[] causedErrors)
        : this(exception.Message, exception, causedErrors)
    {
    }

    /// <inheritdoc/>
    public virtual bool Equals(ResultError? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return EqualityComparer<string>.Default.Equals(Message, other.Message)
               && EqualityComparer<Exception>.Default.Equals(Exception, other.Exception)
               && _causedErrors.SequenceEqual(other._causedErrors);
    }

    /// <summary>
    /// Convert to human-readable representation
    /// </summary>
    /// <returns>Human-readable string based on result state</returns>
    [ExcludeFromCodeCoverage]
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

        if (CausedErrors is { Count: > 0 })
        {
            builder.Append(", CausedErrors = [ ");
            builder.AppendJoin("; ", CausedErrors);
            builder.Append(" ]");
        }

        return true;
    }
}
