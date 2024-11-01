using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;
using System.Text.Json;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

public class JsonDateTimeConverterAttributeTests
{
    [Fact]
    public void SerializeAndDeserialize_DateTime_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss").CreateConverter(typeof(DateTime)) }
        };
        var originalDate = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateTime>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"2023-10-01T12:00:00\"", json);
    }

    [Fact]
    public void SerializeAndDeserialize_NullableDateTime_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss").CreateConverter(typeof(DateTime?)) }
        };
        DateTime? originalDate = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"2023-10-01T12:00:00\"", json);
    }

    [Fact]
    public void SerializeAndDeserialize_DateTimeOffset_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset)) }
        };
        var originalDate = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"2023-10-01T12:00:00.000Z\"", json);
    }

    [Fact]
    public void SerializeAndDeserialize_NullableDateTimeOffset_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset?)) }
        };
        DateTimeOffset? originalDate = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"2023-10-01T12:00:00.000Z\"", json);
    }

    [Fact]
    public void SerializeAndDeserialize_TestModel_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new TestModelJsonDateTimeConverter
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero)
        };
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00",
                                      "NullableDateTimeProperty": "2023-10-01T12:00:00",
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "NullableDateTimeOffsetProperty": "2023-10-01T12:00:00.000Z"
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, options);
        var deserializedModel = JsonSerializer.Deserialize<TestModelJsonDateTimeConverter>(json, options);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SerializeAndDeserialize_TestModel_WithNullValues_ShouldMatchOriginal()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new TestModelJsonDateTimeConverter
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = null,
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = null
        };
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00",
                                      "NullableDateTimeProperty": null,
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "NullableDateTimeOffsetProperty": null
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, options);
        var deserializedModel = JsonSerializer.Deserialize<TestModelJsonDateTimeConverter>(json, options);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Null(deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Null(deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }
}