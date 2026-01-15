namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating JsonDateTimeFormatAttribute usage with DateTimeConverterResolver (.NET 9+).
/// This model uses the JsonDateTimeFormatAttribute (derives from Attribute only) which, when combined with DateTimeConverterResolver,
/// works with source generators in .NET 9 and above without producing SYSLIB1223 warnings.
/// </summary>
public class SourceGeneratorWithResolverFormatModel
{
    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss")]
    public DateTime DateTimeProperty { get; set; }

    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? NullableDateTimeProperty { get; set; }

    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset DateTimeOffsetProperty { get; set; }

    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset? NullableDateTimeOffsetProperty { get; set; }
}
