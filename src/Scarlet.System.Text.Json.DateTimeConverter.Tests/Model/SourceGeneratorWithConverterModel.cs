using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating JsonDateTimeFormatConverter usage with source generators.
/// This model uses the JsonConverter attribute with JsonDateTimeFormatConverter which works
/// with both source generator and reflection-based serialization.
/// </summary>
public class SourceGeneratorWithConverterModel
{
    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeFormat>))]
    public DateTime DateTimeProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeFormat>))]
    public DateTime? NullableDateTimeProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeOffsetFormat>))]
    public DateTimeOffset DateTimeOffsetProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeOffsetFormat>))]
    public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateOnlyFormat>))]
    public DateOnly DateOnlyProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateOnlyFormat>))]
    public DateOnly? NullableDateOnlyProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.TimeOnlyFormat>))]
    public TimeOnly TimeOnlyProperty { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.TimeOnlyFormat>))]
    public TimeOnly? NullableTimeOnlyProperty { get; set; }
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
    internal class DateOnlyFormat : IJsonDateTimeFormat
    {
        public static string Format => "MM/dd/yyyy";
    }
    internal class TimeOnlyFormat : IJsonDateTimeFormat
    {
        public static string Format => "HH.mm.ss";
    }
}