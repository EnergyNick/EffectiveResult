using System.Diagnostics.CodeAnalysis;

namespace EffectiveResult.Exceptions;

/// <summary>
/// Thrown when trying to create or change result to incorrect state
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Only store data without any logic")]
public class InvalidResultOperationException : ResultException
{
    /// <inheritdoc />
    public InvalidResultOperationException(string message) : base(message)
    { }

    /// <inheritdoc />
    public InvalidResultOperationException(string message, Exception innerException) : base(message, innerException)
    { }
}
