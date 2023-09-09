namespace SimpleResult.Exceptions;

/// <summary>
/// Base exception for all incorrect operations with result
/// </summary>
public class ArgumentNullOnSuccessException : ResultException
{
    private const string ExceptionMessage = "Result is in success status, but not provided value for Result";

    /// <inheritdoc />
    /// <param name="argumentName">Name of argument, that is null</param>
    public ArgumentNullOnSuccessException(string argumentName) : base(FailedResultMessage(argumentName))
    { }

    /// <inheritdoc />
    /// <param name="argumentName">Name of operation, that threw the exception</param>
    /// <param name="message">Error reason of exception</param>
    public ArgumentNullOnSuccessException(string argumentName, string message)
        : base(FailedResultMessage(argumentName) + message)
    { }

    private static string FailedResultMessage(string? argumentName) =>
        argumentName != null
            ? $"{ExceptionMessage}: {argumentName}."
            : ExceptionMessage;
}