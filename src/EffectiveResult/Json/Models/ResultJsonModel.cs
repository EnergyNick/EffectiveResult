using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

public sealed class ResultJsonModel
{
    public bool IsSuccess { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyCollection<ResultError>? Errors { get; init; }
}
