namespace EffectiveResult.Abstractions;

/// <summary>
/// Represent object with inner storing value
/// </summary>
/// <typeparam name="TValue">Type of storing value</typeparam>
public interface IValueStorage<TValue>
{
    /// <summary>
    /// Return reference to storing value, if it exists or null
    /// </summary>
    ref readonly TValue? ValueOrDefault { get; }

    /// <summary>
    /// Return reference to storing value or throw exception, if value is not provided
    /// </summary>
    ref readonly TValue Value { get; }
}