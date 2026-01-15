using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating JsonDateTimeFormatConverter usage with source generators.
/// This model uses the JsonConverter attribute with JsonDateTimeFormatConverter which works
/// with both source generator and reflection-based serialization.
/// </summary>
public class SourceGeneratorModel
{
    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeFormat>))]
    public DateTime DateTimeProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeFormat>))]
    public DateTime? NullableDateTimeProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeOffsetFormat>))]
    public DateTimeOffset DateTimeOffsetProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeOffsetFormat>))]
    public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }
}

internal class JsonDateTimeFormat
{
    internal class DateTimeOffsetFormat : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss.fffZ";
    }
    internal class DateTimeFormat : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss";
    }
}