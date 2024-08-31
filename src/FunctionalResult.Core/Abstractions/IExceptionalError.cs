namespace FunctionalResult.Abstractions;

/// <summary>
/// Represents the base type of error causes by exception.
/// </summary>
public interface IExceptionalError : IError
{
    /// <summary>
    /// Caused exception from operation.
    /// </summary>
    Exception Exception { get; }
}