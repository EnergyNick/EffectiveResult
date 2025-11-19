using System.Text.Json;
using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

internal class ResultJsonConverter : JsonConverter<Result>
{
    public override Result? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonModel = JsonSerializer.Deserialize<ResultJsonModel>(ref reader, options);

        if (jsonModel is null)
        {
            return null;
        }

        if (jsonModel.IsSuccess)
        {
            // Ignore any errors for more safety parsing
            return Result.Ok();
        }
        else
        {
            if (jsonModel.Errors is not { Count: > 0 })
            {
                // Failed result MUST have any errors
                return null;
            }
            return Result.Fail(jsonModel.Errors);
        }
    }

    public override void Write(Utf8JsonWriter writer, Result value, JsonSerializerOptions options)
    {
        var data = value.IsSuccess
            ? new ResultJsonModel { IsSuccess = true }
            : new ResultJsonModel { IsSuccess = false, Errors = value.Errors };

        JsonSerializer.Serialize(writer, data);
    }
}
