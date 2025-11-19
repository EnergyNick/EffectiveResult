using System.Text.Json;
using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

internal sealed class ResultErrorJsonConverter : JsonConverter<ResultError>
{
    public override ResultError? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonModel = JsonSerializer.Deserialize<ResultErrorJsonModel>(ref reader, options);

        return jsonModel is not null
            ? ConvertFromJsonModel(jsonModel)
            : null;
    }

    public override void Write(Utf8JsonWriter writer, ResultError value, JsonSerializerOptions options)
    {
        var data = ConvertToJsonModel(value);

        JsonSerializer.Serialize(writer, data);
    }

    private ResultError ConvertFromJsonModel(ResultErrorJsonModel error) =>
        new(
            error.Message,
            error.Exception,
            error.CausedErrors?.Select(ConvertFromJsonModel).ToArray());

    private ResultErrorJsonModel ConvertToJsonModel(ResultError error) =>
        new()
        {
            Message = error.Message,
            Exception = error.Exception,
            CausedErrors = error.CausedErrors is { Count: > 0 }
                ? error.CausedErrors.Select(ConvertToJsonModel).ToArray()
                : null
        };
}
