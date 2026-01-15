using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Converters;

/// <summary>
/// Converts nullable <see cref="DateOnly"/> objects to and from JSON using a specified date format.
/// </summary>
internal sealed class DateOnlyNullableConverter : JsonConverter<DateOnly?>
{
    private readonly string _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyNullableConverter"/> class with the specified date format.
    /// </summary>
    /// <param name="format">The date format string.</param>
    private DateOnlyNullableConverter(string format) => _format = format;

    /// <summary>
    /// Reads and converts the JSON to a nullable <see cref="DateOnly"/> object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted nullable <see cref="DateOnly"/> object, or null if the JSON token is null.</returns>
    public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateOnly.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly date))
            {
                return date;
            }
        }

        return null;
    }

    /// <summary>
    /// Writes a nullable <see cref="DateOnly"/> object as a JSON string using the specified date format.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The nullable <see cref="DateOnly"/> value to write.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.HasValue)
        {
            var date = value.Value.ToString(_format, CultureInfo.InvariantCulture);
            writer.WriteStringValue(date);
        }
        else
        {
            writer.WriteNullValue();
        }
    }

    public static DateOnlyNullableConverter FromFormat(string format) => new(format);
}
