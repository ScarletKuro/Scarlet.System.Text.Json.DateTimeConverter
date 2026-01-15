namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating JsonDateTimeConverter attribute usage with DateTimeConverterResolver (.NET 9+).
/// This model uses the JsonDateTimeConverter attribute (derives from JsonConverterAttribute) which, when combined with DateTimeConverterResolver,
/// works with source generators in .NET 9 and above. Note: This produces SYSLIB1223 warnings but still works.
/// </summary>
public class SourceGeneratorWithResolverAttributeModel
{
    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime DateTimeProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? NullableDateTimeProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset DateTimeOffsetProperty { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }
}