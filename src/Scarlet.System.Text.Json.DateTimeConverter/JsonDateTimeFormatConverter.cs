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

        if (typeToConvert == typeof(DateTime))
        {
            return Converters.DateTimeConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(DateTime?))
        {
            return Converters.DateTimeNullableConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(DateTimeOffset))
        {
            return Converters.DateTimeOffsetConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(DateTimeOffset?))
        {
            return Converters.DateTimeOffsetNullableConverter.FromFormat(format);
        }

        throw new NotSupportedException($"{typeToConvert.FullName} is not supported by the {nameof(JsonDateTimeConverterAttribute)}.");
    }
}



