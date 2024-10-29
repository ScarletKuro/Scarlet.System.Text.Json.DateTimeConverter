using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Converters;

/// <summary>
/// Converts <see cref="DateTimeOffsetConverter"/> objects to and from JSON using a specified date format.
/// </summary>
internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    private readonly string _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetConverter"/> class with the specified date format.
    /// </summary>
    /// <param name="format">The date format string.</param>
    private DateTimeOffsetConverter(string format) => _format = format;

    /// <summary>
    /// Reads and converts the JSON to a <see cref="DateTimeOffset"/> object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted <see cref="DateTimeOffset"/> object.</returns>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTimeOffset.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeOffset date))
            {
                return date;
            }
        }

        return reader.GetDateTimeOffset();
    }

    /// <summary>
    /// Writes a <see cref="DateTimeOffset"/> object as a JSON string using the specified date format.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The <see cref="DateTimeOffset"/> value to write.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        var date = value.ToString(_format, CultureInfo.InvariantCulture);
        writer.WriteStringValue(date);
    }

    public static DateTimeOffsetConverter FromFormat(string format) => new(format);
}