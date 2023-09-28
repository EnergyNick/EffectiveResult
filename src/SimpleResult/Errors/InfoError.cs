using SimpleResult.Core;

namespace SimpleResult;

/// <summary>
/// Represent error with informal message on unsuccessful operation.
/// </summary>
public record InfoError : BaseError
{
    public InfoError(string message) : base(message)
    { }

    public InfoError(string message, IError causedBy) : base(message, causedBy)
    { }

    public InfoError(string message, params IError[] causedBy) : base(message, causedBy)
    { }

    public InfoError(string message, IEnumerable<IError> causedBy) : base(message, causedBy)
    { }
}