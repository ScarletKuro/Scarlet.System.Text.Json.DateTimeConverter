using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter;

internal static class DateTimeConverterFactoryHelper
{
    public static JsonConverter CreateConverter(Type typeToConvert, string format)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

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

        if (typeToConvert == typeof(DateOnly))
        {
            return Converters.DateOnlyConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(DateOnly?))
        {
            return Converters.DateOnlyNullableConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(TimeOnly))
        {
            return Converters.TimeOnlyConverter.FromFormat(format);
        }

        if (typeToConvert == typeof(TimeOnly?))
        {
            return Converters.TimeOnlyNullableConverter.FromFormat(format);
        }

        throw new NotSupportedException($"{typeToConvert.FullName} is not supported by the {nameof(DateTimeConverterFactoryHelper)}.");
    }
}