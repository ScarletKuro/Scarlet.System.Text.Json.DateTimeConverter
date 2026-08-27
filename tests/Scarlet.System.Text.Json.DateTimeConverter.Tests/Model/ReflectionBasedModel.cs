namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// Test model demonstrating JsonDateTimeConverter attribute usage with reflection-based serialization.
/// This model uses the JsonDateTimeConverter attribute which only works with reflection-based
/// System.Text.Json serialization (not with source generators).
/// </summary>
public class ReflectionBasedModel
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