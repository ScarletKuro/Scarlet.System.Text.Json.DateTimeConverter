using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter;

/// <summary>
/// A JSON converter factory that creates converters for <see cref="DateTime"/> and <see cref="DateTimeOffset"/> types using a specified date format.
/// </summary>
/// <typeparam name="T">The type that implements <see cref="IJsonDateTimeFormat"/> to provide the date format.</typeparam>
/// <remarks>
/// This converter is used when you want to use source generator `System.Text.Json`, but it also works with non-source generator (reflection-based) serialization and deserialization.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// public class Model
/// {
///     [JsonConverter(typeof(JsonDateTimeFormatConverter&lt;JsonDateTimeFormat.DateTimeOffsetFormat&gt;))]
///     public DateTimeOffset DateTimeOffsetProperty { get; set; }
/// }
/// internal class JsonDateTimeFormat
/// {
///     internal class DateTimeOffsetFormat : IJsonDateTimeFormat
///     {
///         public static string Format => "yyyy-MM-ddTHH:mm:ss.fffZ";
///     }
/// }
/// </code>
/// </example>
public class JsonDateTimeFormatConverter<T> : JsonConverterFactory where T : IJsonDateTimeFormat
{
    /// <summary>
    /// Determines whether the specified type can be converted.
    /// </summary>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <returns><c>true</c> if the type can be converted; otherwise, <c>false</c>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(DateTime) ||
               typeToConvert == typeof(DateTime?) ||
               typeToConvert == typeof(DateTimeOffset) ||
               typeToConvert == typeof(DateTimeOffset?);
    }

    /// <summary>
    /// Creates a converter for the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>A <see cref="JsonConverter"/> for the specified type.</returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        var format = T.Format;
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeToConvert, format);

        return converter;
    }
}
