using System.Text.Json;
using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

public class JsonDateTimeFormatConverterTests
{
    [Fact]
    public void ReflectionBased_CompleteModel_WithFormatConverter()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new SourceGeneratorWithConverterModel
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
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorWithConverterModel>(json, options);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void ReflectionBased_CompleteModel_WithFormatConverter_WithNullValues()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new SourceGeneratorWithConverterModel
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
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorWithConverterModel>(json, options);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Null(deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Null(deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_CompleteModel_WithFormatConverter()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorWithConverterModel);
        var context = ConverterModelJsonSerializerContext.Default;
        var originalModel = new SourceGeneratorWithConverterModel
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
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorWithConverterModel?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_CompleteModel_WithFormatConverter_WithNullValues()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorWithConverterModel);
        var context = ConverterModelJsonSerializerContext.Default;
        var originalModel = new SourceGeneratorWithConverterModel
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
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorWithConverterModel?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Null(deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Null(deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(expectedJson, json);
    }
}