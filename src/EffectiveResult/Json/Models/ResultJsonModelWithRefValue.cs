using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

public sealed class ResultJsonModelWithRefValue<T>
    where T : class
{
    public bool IsSuccess { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Value { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyCollection<ResultError>? Errors { get; init; }
}
