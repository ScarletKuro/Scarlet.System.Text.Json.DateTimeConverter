using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Converters;

/// <summary>
/// Converts nullable <see cref="TimeOnly"/> objects to and from JSON using a specified time format.
/// </summary>
internal sealed class TimeOnlyNullableConverter : JsonConverter<TimeOnly?>
{
    private readonly string _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyNullableConverter"/> class with the specified time format.
    /// </summary>
    /// <param name="format">The time format string.</param>
    private TimeOnlyNullableConverter(string format) => _format = format;

    /// <summary>
    /// Reads and converts the JSON to a nullable <see cref="TimeOnly"/> object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted nullable <see cref="TimeOnly"/> object, or null if the JSON token is null.</returns>
    public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (TimeOnly.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly time))
            {
                return time;
            }
        }

        return null;
    }

    /// <summary>
    /// Writes a nullable <see cref="TimeOnly"/> object as a JSON string using the specified time format.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The nullable <see cref="TimeOnly"/> value to write.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value.HasValue)
        {
            var time = value.Value.ToString(_format, CultureInfo.InvariantCulture);
            writer.WriteStringValue(time);
        }
        else
        {
            writer.WriteNullValue();
        }
    }

    public static TimeOnlyNullableConverter FromFormat(string format) => new(format);
}
