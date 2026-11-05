using System.Text.Json;
using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Backward compatibility: <see cref="JsonDateTimeConverterAttribute"/> on a source-generated model. The source
/// generator ignores the attribute (SYSLIB1223); <see cref="DateTimeConverterResolver"/> applies it at runtime.
/// These tests also run under NativeAOT with serializer reflection disabled.
/// </summary>
public partial class SourceGeneratorBasedTests
{
    private static SourceGeneratorModelWithConverterAttribute CreateConverterAttributeModel(bool withNulls) => new()
    {
        DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
        NullableDateTimeProperty = withNulls ? null : new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
        DateTimeOffsetProperty = new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
        NullableDateTimeOffsetProperty = withNulls ? null : new DateTimeOffset(2023, 10, 1, 12, 0, 0, TimeSpan.Zero),
        DateOnlyProperty = new DateOnly(2023, 10, 1),
        NullableDateOnlyProperty = withNulls ? null : new DateOnly(2023, 10, 1),
        TimeOnlyProperty = new TimeOnly(14, 30, 45),
        NullableTimeOnlyProperty = withNulls ? null : new TimeOnly(14, 30, 45)
    };

    private static void AssertRoundTrip(SourceGeneratorModelWithConverterAttribute expected, SourceGeneratorModelWithConverterAttribute? actual)
    {
        Assert.NotNull(actual);
        Assert.Equal(expected.DateTimeProperty, actual.DateTimeProperty);
        Assert.Equal(expected.NullableDateTimeProperty, actual.NullableDateTimeProperty);
        Assert.Equal(expected.DateTimeOffsetProperty, actual.DateTimeOffsetProperty);
        Assert.Equal(expected.NullableDateTimeOffsetProperty, actual.NullableDateTimeOffsetProperty);
        Assert.Equal(expected.DateOnlyProperty, actual.DateOnlyProperty);
        Assert.Equal(expected.NullableDateOnlyProperty, actual.NullableDateOnlyProperty);
        Assert.Equal(expected.TimeOnlyProperty, actual.TimeOnlyProperty);
        Assert.Equal(expected.NullableTimeOnlyProperty, actual.NullableTimeOnlyProperty);
    }

    [Fact]
    public void SourceGenerator_WithResolver_WithConverterAttribute_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DateTimeConverterResolver(ConverterAttributeModelJsonSerializerContext.Default)
        };
        var originalModel = CreateConverterAttributeModel(withNulls: false);
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
        var json = TestJson.Serialize(originalModel, sourceGenOptions);
        var deserializedModel = TestJson.Deserialize<SourceGeneratorModelWithConverterAttribute>(json, sourceGenOptions);

        // Assert
        Assert.Equal(expectedJson, json);
        AssertRoundTrip(originalModel, deserializedModel);
    }

    [Fact]
    public void SourceGenerator_WithResolver_WithConverterAttribute_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorModelWithConverterAttribute);
        var context = new DateTimeConverterResolver(ConverterAttributeModelJsonSerializerContext.Default);
        var originalModel = CreateConverterAttributeModel(withNulls: false);
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
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorModelWithConverterAttribute?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.Equal(expectedJson, json);
        AssertRoundTrip(originalModel, deserializedModel);
    }

    [Fact]
    public void SourceGenerator_WithResolver_WithConverterAttribute_WithNullValues_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorModelWithConverterAttribute);
        var context = new DateTimeConverterResolver(ConverterAttributeModelJsonSerializerContext.Default);
        var originalModel = CreateConverterAttributeModel(withNulls: true);
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
        var deserializedModel = (SourceGeneratorModelWithConverterAttribute?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.Equal(expectedJson, json);
        AssertRoundTrip(originalModel, deserializedModel);
    }

    [Fact]
    public void SourceGenerator_WithoutResolver_WithConverterAttribute_DoesNotApplyFormat()
    {
        // Arrange: the source generator ignores JsonConverterAttribute-derived attributes, so without the resolver
        // the built-in ISO 8601 formats are used.
        var testModelType = typeof(SourceGeneratorModelWithConverterAttribute);
        var context = ConverterAttributeModelJsonSerializerContext.Default;
        var originalModel = CreateConverterAttributeModel(withNulls: false);
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
        var deserializedModel = (SourceGeneratorModelWithConverterAttribute?)JsonSerializer.Deserialize(json, testModelType, context);

        // Assert
        Assert.Equal(expectedJson, json);
        AssertRoundTrip(originalModel, deserializedModel);
    }
}
