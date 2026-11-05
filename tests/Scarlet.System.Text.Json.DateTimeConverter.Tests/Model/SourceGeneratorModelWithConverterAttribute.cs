namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating backward-compatible JsonDateTimeConverterAttribute usage with DateTimeConverterResolver (.NET 9+).
/// The source generator ignores attributes deriving from JsonConverterAttribute and reports SYSLIB1223 for them;
/// DateTimeConverterResolver applies them at runtime through JsonPropertyInfo.AttributeProvider.
/// </summary>
public class SourceGeneratorModelWithConverterAttribute
{
    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime DateTimeProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? NullableDateTimeProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset DateTimeOffsetProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }

    [JsonDateTimeConverter("MM/dd/yyyy")]
    public DateOnly DateOnlyProperty { get; set; }

    [JsonDateTimeConverter("MM/dd/yyyy")]
    public DateOnly? NullableDateOnlyProperty { get; set; }

    [JsonDateTimeConverter("HH.mm.ss")]
    public TimeOnly TimeOnlyProperty { get; set; }

    [JsonDateTimeConverter("HH.mm.ss")]
    public TimeOnly? NullableTimeOnlyProperty { get; set; }
}
