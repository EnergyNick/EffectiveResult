namespace EffectiveResult.Abstractions;

public interface IReferenceValueStorage<TValue>
{
    /// <summary>
    /// Return reference to storing value, if it exists or null
    /// </summary>
    ref readonly TValue? ValueOrDefaultRef { get; }

    /// <summary>
    /// Return reference to storing value or throw exception, if value is not provided
    /// </summary>
    ref readonly TValue ValueRef { get; }
}
