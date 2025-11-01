using System.Text;

namespace EffectiveResult;

/// <summary>
/// Represents the base type of error causes by exception.
/// </summary>
public record ExceptionalError : Error
{
    /// <summary>
    /// Caused exception from operation.
    /// </summary>
    public Exception Exception { get; init; }

    public ExceptionalError(Exception exception)
        : base(exception.Message)
    {
        Exception = exception;
    }

    public ExceptionalError(string message, Exception exception)
        : base(message)
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception, Error causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception, params Error[] causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception, IEnumerable<Error> causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    protected override bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Message = '");
        builder.Append(Message);
        builder.Append('\'');

        builder.Append(", Exception = '");
        builder.Append(Message);
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
