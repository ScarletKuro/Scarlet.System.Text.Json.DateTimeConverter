namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

public class TestModel
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