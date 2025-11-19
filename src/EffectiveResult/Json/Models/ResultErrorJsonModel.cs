using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

public sealed class ResultErrorJsonModel
{
    public required string Message { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Exception? Exception { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IReadOnlyCollection<ResultErrorJsonModel>? CausedErrors { get; init; }
}
