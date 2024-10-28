using System.Text.Json.Serialization;
using DateTime = System.DateTime;

namespace Scarlet.System.Text.Json.DateTimeConverter;

/// <summary>
/// Specifies that a <see cref="DateTime"/>, <see cref="DateTimeOffset"/> (including Nullables) should be converted using a custom date format.
/// </summary>
public sealed class JsonDateTimeConverterAttribute : JsonConverterAttribute
{
    private readonly string _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDateTimeConverterAttribute"/> class with the specified date format.
    /// </summary>
    /// <param name="format">The date format string.</param>
    public JsonDateTimeConverterAttribute(string format) => _format = format;

    /// <summary>
    /// Creates a <see cref="JsonConverter"/> for the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <returns>A <see cref="JsonConverter"/> for the specified type.</returns>
    /// <exception cref="NotSupportedException">Thrown when the type to convert is not <see cref="DateTime"/>.</exception>
    public override JsonConverter CreateConverter(Type typeToConvert)
    {
        if (typeToConvert == typeof(DateTime))
        {
            return new Converters.DateTimeConverter(_format);
        }

        if (typeToConvert == typeof(DateTime?))
        {
            return new Converters.DateTimeNullableConverter(_format);
        }

        if (typeToConvert == typeof(DateTimeOffset))
        {
            return new Converters.DateTimeOffsetConverter(_format);
        }

        if (typeToConvert == typeof(DateTimeOffset?))
        {
            return new Converters.DateTimeOffsetNullableConverter(_format);
        }

        throw new NotSupportedException($"{typeToConvert.FullName} is not supported by the {nameof(JsonDateTimeConverterAttribute)}.");
    }
}
