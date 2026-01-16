using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter;

/// <summary>
/// Specifies a custom date format for <see cref="DateTime"/>, <see cref="DateTimeOffset"/> (including Nullables) when used with source generators and <see cref="DateTimeConverterResolver"/>.
/// </summary>
/// <remarks>
/// This attribute is specifically designed for use with .NET 9+ source generators and <see cref="DateTimeConverterResolver"/>.
/// Unlike <see cref="JsonDateTimeConverterAttribute"/>, this attribute does not derive from JsonConverterAttribute and will not produce SYSLIB1223 warnings.
/// For reflection-based serialization, use <see cref="JsonDateTimeConverterAttribute"/> instead.
/// </remarks>
/// <example>
/// Example usage with source generator:
/// <code>
/// public class Model
/// {
///     [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ")]
///     public DateTimeOffset DateTimeOffsetProperty { get; set; }
/// }
/// 
/// [JsonSerializable(typeof(Model))]
/// public partial class MyJsonContext : JsonSerializerContext { }
/// 
/// // Usage:
/// var options = new JsonSerializerOptions
/// {
///     TypeInfoResolver = new DateTimeConverterResolver(MyJsonContext.Default)
/// };
/// var json = JsonSerializer.Serialize(model, options);
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class JsonDateTimeFormatAttribute : JsonAttribute
{
    /// <summary>
    /// Gets the date format string.
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonDateTimeFormatAttribute"/> class with the specified date format.
    /// </summary>
    /// <param name="format">The date format string.</param>
    public JsonDateTimeFormatAttribute(string format)
    {
        ArgumentNullException.ThrowIfNull(format);
        Format = format;
    }
}
