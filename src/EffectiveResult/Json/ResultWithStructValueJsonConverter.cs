using System.Text.Json;
using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

internal class ResultWithStructValueJsonConverter<T> : JsonConverter<Result<T>>
    where T : struct
{
    public override Result<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonModel = JsonSerializer.Deserialize<ResultJsonModelWithValue<T>>(ref reader, options);

        if (jsonModel is null)
        {
            return null;
        }

        if (jsonModel.IsSuccess)
        {
            // Ignore any errors for more safety parsing
            return Result.Ok(jsonModel.Value.GetValueOrDefault());
        }
        else
        {
            if (jsonModel.Errors is not { Count: > 0 })
            {
                // Failed result MUST have any errors
                return null;
            }
            return Result.Fail<T>(jsonModel.Errors);
        }
    }

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
        var data = value.IsSuccess
            ? new ResultJsonModelWithValue<T> { IsSuccess = true, Value = value.ValueOrDefault }
            : new ResultJsonModelWithValue<T> { IsSuccess = false, Errors = value.Errors };

        JsonSerializer.Serialize(writer, data, options);
    }
}
