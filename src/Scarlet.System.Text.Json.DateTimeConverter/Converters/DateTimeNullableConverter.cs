using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Converters;

/// <summary>
/// Converts nullable <see cref="DateTime"/> objects to and from JSON using a specified date format.
/// </summary>
internal class DateTimeNullableConverter : JsonConverter<DateTime?>
{
    private readonly string _format;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeNullableConverter"/> class with the specified date format.
    /// </summary>
    /// <param name="format">The date format string.</param>
    public DateTimeNullableConverter(string format) => _format = format;

    /// <summary>
    /// Reads and converts the JSON to a nullable <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="reader">The <see cref="Utf8JsonReader"/> to read from.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    /// <returns>The converted nullable <see cref="DateTime"/> object, or null if the JSON token is null.</returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTime.TryParseExact(reader.GetString(), _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }
        }

        return null;
    }

    /// <summary>
    /// Writes a nullable <see cref="DateTime"/> object as a JSON string using the specified date format.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write to.</param>
    /// <param name="value">The nullable <see cref="DateTime"/> value to write.</param>
    /// <param name="options">Options to control the conversion behavior.</param>
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
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
}
