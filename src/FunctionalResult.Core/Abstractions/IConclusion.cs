namespace FunctionalResult.Abstractions;

/// <summary>
/// The base type of results, represent only final state of operation.
/// Used for core abstraction of operations results.
/// </summary>
public interface IConclusion
{
    /// <summary>
    /// Is true, if contains no errors.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Is true, if contains any errors.
    /// </summary>
    bool IsFailed { get; }

    /// <summary>
    /// Get all errors.
    /// </summary>
    IReadOnlyCollection<IError> Errors { get; }
}