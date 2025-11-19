using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EffectiveResult.Json;

internal class ResultWithValueJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var storingType = typeToConvert.GetGenericArguments()[0];
        var typeOfConverter = storingType.IsClass
            ? typeof(ResultWithClassValueJsonConverter<>)
            : typeof(ResultWithStructValueJsonConverter<>);

        return (JsonConverter?)Activator.CreateInstance(
            typeOfConverter.MakeGenericType(storingType),
            BindingFlags.Instance | BindingFlags.Public,
            args: [],
            binder: null,
            culture: null);
    }
}
