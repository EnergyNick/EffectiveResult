namespace SimpleResult.Core;

public interface IValueProvider<TValue>
{
    ref readonly TValue? ValueOrDefault { get; }

    ref readonly TValue Value { get; }
}