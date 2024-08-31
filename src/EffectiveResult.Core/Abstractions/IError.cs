namespace EffectiveResult.Abstractions;

/// <summary>
/// Represents the base type of all error causes.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets a message that describes the current error.
    /// </summary>
    /// <returns>The error message that explains the reason, or an empty string ("").</returns>
    string Message { get; }

    /// <summary>
    /// Errors causing this error.
    /// </summary>
    IReadOnlyCollection<IError> CausedErrors { get; }
}