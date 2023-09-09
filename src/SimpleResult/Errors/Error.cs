using System.Collections.Immutable;
using SimpleResult.Core;

namespace SimpleResult;

public record Error(string Message) : IError
{
    private readonly ImmutableArray<IError> _causedErrors = ImmutableArray<IError>.Empty;

    public IReadOnlyCollection<IError> CausedErrors => _causedErrors;

    public Error(string message, IError causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    public Error(string message, params IError[] causedBy) : this(message) =>
        _causedErrors = ImmutableArray.Create(causedBy);

    public Error(string message, IEnumerable<IError> causedBy) : this(message) =>
        _causedErrors = causedBy.ToImmutableArray();
}