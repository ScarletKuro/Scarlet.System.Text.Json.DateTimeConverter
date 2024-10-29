using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter;

public class JsonDateTimeFormatConverter<T> : JsonConverterFactory where T : IJsonDateTimeFormat
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) ||
               typeToConvert == typeof(DateTime?) ||
               typeToConvert == typeof(DateTimeOffset) ||
               typeToConvert == typeof(DateTimeOffset?);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        var format = T.Format;
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeToConvert, format);

        return converter;
    }
}



