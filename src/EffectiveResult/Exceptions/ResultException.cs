using System.Diagnostics.CodeAnalysis;

namespace EffectiveResult.Exceptions;

/// <summary>
/// Base exception for all incorrect operations with result
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Only store data without any logic")]
public class ResultException : Exception
{
    /// <inheritdoc />
    public ResultException(string message) : base(message)
    { }

    /// <inheritdoc />
    public ResultException(string message, Exception innerException) : base(message, innerException)
    { }
}
