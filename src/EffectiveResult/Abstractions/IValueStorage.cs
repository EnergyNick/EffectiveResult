namespace EffectiveResult.Abstractions;

/// <summary>
/// Represent object with inner storing value
/// </summary>
/// <typeparam name="TValue">Type of storing value</typeparam>
public interface IValueStorage<out TValue>
{
    /// <summary>
    /// Return reference to storing value, if it exists or null
    /// </summary>
    TValue? ValueOrDefault { get; }

    /// <summary>
    /// Return reference to storing value or throw exception, if value is not provided
    /// </summary>
    TValue Value { get; }
}
