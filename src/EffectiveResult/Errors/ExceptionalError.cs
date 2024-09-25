using System.Text;
using EffectiveResult.Abstractions;

namespace EffectiveResult;

public record ExceptionalError : Error, IExceptionalError
{
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

    public ExceptionalError(Exception exception, IError causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception, params IError[] causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    public ExceptionalError(Exception exception, IEnumerable<IError> causedBy)
        : base(exception.Message, causedBy)
    {
        Exception = exception;
    }

    protected override bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Message = '");
        builder.Append(Message);
        builder.Append('\'');

        builder.Append("Exception = '");
        builder.Append(Message);
        builder.Append('\'');

        if (CausedErrors.Any())
        {
            builder.Append(", CausedErrors = [ ");
            builder.AppendJoin("; ", CausedErrors);
            builder.Append(" ]");
        }
        return true;
    }
}