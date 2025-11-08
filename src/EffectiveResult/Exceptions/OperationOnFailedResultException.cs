using System.Diagnostics.CodeAnalysis;

namespace EffectiveResult.Exceptions;

/// <summary>
/// Thrown when operation invoked on unsuccessful result
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Only store data without any logic")]
public class OperationOnFailedResultException : ResultException
{
    private const string ExceptionMessage = "Result is in failed status, can't do operation";

    /// <inheritdoc />
    /// <param name="operationName">Name of operation, that threw the exception</param>
    public OperationOnFailedResultException(string operationName) : base(FailedResultMessage(operationName))
    { }

    /// <inheritdoc />
    /// <param name="operationName">Name of operation, that threw the exception</param>
    /// <param name="innerException">The exception that is the cause of the current exception</param>
    public OperationOnFailedResultException(string operationName, Exception innerException)
        : base(FailedResultMessage(operationName), innerException)
    { }

    /// <inheritdoc />
    /// <param name="message">The message that describes the error.</param>
    /// <param name="operationName">Name of operation, that threw the exception</param>
    public OperationOnFailedResultException(string message, string operationName)
        : base(FailedResultMessage(operationName) + message)
    { }

    /// <inheritdoc />
    public OperationOnFailedResultException(string message, string operationName, Exception innerException)
        : base(FailedResultMessage(operationName) + message, innerException)
    { }

    private static string FailedResultMessage(string? operationName) =>
        operationName != null
            ? $"{ExceptionMessage}: {operationName}."
            : ExceptionMessage;
}
