#pragma warning disable CS1573

namespace FunctionalResult.Exceptions;

/// <summary>
/// Thrown when trying to create or change result to incorrect state
/// </summary>
public class InvalidResultOperationException : ResultException
{
    /// <inheritdoc />
    public InvalidResultOperationException(string message) : base(message)
    { }

    /// <inheritdoc />
    public InvalidResultOperationException(string message, Exception innerException) : base(message, innerException)
    { }
}