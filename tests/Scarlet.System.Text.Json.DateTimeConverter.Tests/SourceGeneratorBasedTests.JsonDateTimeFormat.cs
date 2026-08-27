using System.Text.Json;
using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

public partial class SourceGeneratorBasedTests
{
    [Fact]
    public void SourceGenerator_WithResolver_WithFormatAttribute_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default)
        };
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = new DateOnly(2023, 10, 1),
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = new TimeOnly(14, 30, 45)
        };
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00",
                                      "NullableDateTimeProperty": "2023-10-01T12:00:00",
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "NullableDateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "DateOnlyProperty": "10/01/2023",
                                      "NullableDateOnlyProperty": "10/01/2023",
                                      "TimeOnlyProperty": "14.30.45",
                                      "NullableTimeOnlyProperty": "14.30.45"
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, sourceGenOptions);
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorModelWithAttribute>(json, sourceGenOptions);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Equal(originalModel.NullableDateOnlyProperty, deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Equal(originalModel.NullableTimeOnlyProperty, deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_WithResolver_WithFormatAttribute_WithNullValues_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default)
        };
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = null,
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = null,
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = null,
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = null
        };
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00",
                                      "NullableDateTimeProperty": null,
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "NullableDateTimeOffsetProperty": null,
                                      "DateOnlyProperty": "10/01/2023",
                                      "NullableDateOnlyProperty": null,
                                      "TimeOnlyProperty": "14.30.45",
                                      "NullableTimeOnlyProperty": null
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, sourceGenOptions);
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorModelWithAttribute>(json, sourceGenOptions);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Null(deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Null(deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Null(deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Null(deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_WithResolver_WithFormatAttribute_WithNullValues_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorModelWithAttribute);
        var context = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default);
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = null,
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = null,
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = null,
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = null
        };
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00",
                                      "NullableDateTimeProperty": null,
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00.000Z",
                                      "NullableDateTimeOffsetProperty": null,
                                      "DateOnlyProperty": "10/01/2023",
                                      "NullableDateOnlyProperty": null,
                                      "TimeOnlyProperty": "14.30.45",
                                      "NullableTimeOnlyProperty": null
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorModelWithAttribute?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Null(deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Null(deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_WithoutResolver_WithFormatAttribute_DoesNotApplyFormat_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = ResolverModelJsonSerializerContext.Default
        };
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = new DateOnly(2023, 10, 1),
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = new TimeOnly(14, 30, 45)
        };
        // Expected JSON with DEFAULT formats (not the custom formats from JsonDateTimeFormat attribute)
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00Z",
                                      "NullableDateTimeProperty": "2023-10-01T12:00:00Z",
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00+00:00",
                                      "NullableDateTimeOffsetProperty": "2023-10-01T12:00:00+00:00",
                                      "DateOnlyProperty": "2023-10-01",
                                      "NullableDateOnlyProperty": "2023-10-01",
                                      "TimeOnlyProperty": "14:30:45",
                                      "NullableTimeOnlyProperty": "14:30:45"
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, sourceGenOptions);
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorModelWithAttribute>(json, sourceGenOptions);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Equal(originalModel.NullableDateOnlyProperty, deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Equal(originalModel.NullableTimeOnlyProperty, deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_WithoutResolver_WithFormatAttribute_WithNullValues_DoesNotApplyFormat_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = ResolverModelJsonSerializerContext.Default
        };
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = null,
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = null,
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = null,
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = null
        };
        // Expected JSON with DEFAULT formats (not the custom formats from JsonDateTimeFormat attribute)
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00Z",
                                      "NullableDateTimeProperty": null,
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00+00:00",
                                      "NullableDateTimeOffsetProperty": null,
                                      "DateOnlyProperty": "2023-10-01",
                                      "NullableDateOnlyProperty": null,
                                      "TimeOnlyProperty": "14:30:45",
                                      "NullableTimeOnlyProperty": null
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, sourceGenOptions);
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorModelWithAttribute>(json, sourceGenOptions);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Null(deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Null(deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Null(deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Null(deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void SourceGenerator_WithoutResolver_WithFormatAttribute_DoesNotApplyFormat_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorModelWithAttribute);
        var context = ResolverModelJsonSerializerContext.Default;
        var originalModel = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            NullableDateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            NullableDateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
            DateOnlyProperty = new DateOnly(2023, 10, 1),
            NullableDateOnlyProperty = new DateOnly(2023, 10, 1),
            TimeOnlyProperty = new TimeOnly(14, 30, 45),
            NullableTimeOnlyProperty = new TimeOnly(14, 30, 45)
        };
        // Expected JSON with DEFAULT formats (not the custom formats from JsonDateTimeFormat attribute)
        const string expectedJson = """
                                    {
                                      "DateTimeProperty": "2023-10-01T12:00:00Z",
                                      "NullableDateTimeProperty": "2023-10-01T12:00:00Z",
                                      "DateTimeOffsetProperty": "2023-10-01T12:00:00+00:00",
                                      "NullableDateTimeOffsetProperty": "2023-10-01T12:00:00+00:00",
                                      "DateOnlyProperty": "2023-10-01",
                                      "NullableDateOnlyProperty": "2023-10-01",
                                      "TimeOnlyProperty": "14:30:45",
                                      "NullableTimeOnlyProperty": "14:30:45"
                                    }
                                    """;

        // Act
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorModelWithAttribute?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.NotNull(deserializedModel);
        Assert.Equal(originalModel.DateTimeProperty, deserializedModel.DateTimeProperty);
        Assert.Equal(originalModel.NullableDateTimeProperty, deserializedModel.NullableDateTimeProperty);
        Assert.Equal(originalModel.DateTimeOffsetProperty, deserializedModel.DateTimeOffsetProperty);
        Assert.Equal(originalModel.NullableDateTimeOffsetProperty, deserializedModel.NullableDateTimeOffsetProperty);
        Assert.Equal(originalModel.DateOnlyProperty, deserializedModel.DateOnlyProperty);
        Assert.Equal(originalModel.NullableDateOnlyProperty, deserializedModel.NullableDateOnlyProperty);
        Assert.Equal(originalModel.TimeOnlyProperty, deserializedModel.TimeOnlyProperty);
        Assert.Equal(originalModel.NullableTimeOnlyProperty, deserializedModel.NullableTimeOnlyProperty);
        Assert.Equal(expectedJson, json);
    }
}