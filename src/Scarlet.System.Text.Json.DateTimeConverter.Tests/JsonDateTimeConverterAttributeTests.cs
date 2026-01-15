using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;
using System.Text.Json;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

public class JsonDateTimeConverterAttributeTests
{
    [Fact]
    public void ReflectionBased_DateTime_WithAttribute()
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
    public void ReflectionBased_NullableDateTime_WithAttribute()
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
    public void ReflectionBased_DateTimeOffset_WithAttribute()
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
    public void ReflectionBased_NullableDateTimeOffset_WithAttribute()
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
    public void ReflectionBased_DateOnly_WithAttribute()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly)) }
        };
        var originalDate = new DateOnly(2023, 10, 1);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateOnly>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"10/01/2023\"", json);
    }

    [Fact]
    public void ReflectionBased_NullableDateOnly_WithAttribute()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        DateOnly? originalDate = new DateOnly(2023, 10, 1);

        // Act
        var json = JsonSerializer.Serialize(originalDate, options);
        var deserializedDate = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.Equal(originalDate, deserializedDate);
        Assert.Equal("\"10/01/2023\"", json);
    }

    [Fact]
    public void ReflectionBased_TimeOnly_WithAttribute()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH.mm.ss").CreateConverter(typeof(TimeOnly)) }
        };
        var originalTime = new TimeOnly(14, 30, 45);

        // Act
        var json = JsonSerializer.Serialize(originalTime, options);
        var deserializedTime = JsonSerializer.Deserialize<TimeOnly>(json, options);

        // Assert
        Assert.Equal(originalTime, deserializedTime);
        Assert.Equal("\"14.30.45\"", json);
    }

    [Fact]
    public void ReflectionBased_NullableTimeOnly_WithAttribute()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH.mm.ss").CreateConverter(typeof(TimeOnly?)) }
        };
        TimeOnly? originalTime = new TimeOnly(14, 30, 45);

        // Act
        var json = JsonSerializer.Serialize(originalTime, options);
        var deserializedTime = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.Equal(originalTime, deserializedTime);
        Assert.Equal("\"14.30.45\"", json);
    }

    [Fact]
    public void ReflectionBased_CompleteModel_WithAttribute()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new ReflectionBasedModel
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
        var json = JsonSerializer.Serialize(originalModel, options);
        var deserializedModel = JsonSerializer.Deserialize<ReflectionBasedModel>(json, options);

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
    public void ReflectionBased_CompleteModel_WithAttribute_WithNullValues()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var originalModel = new ReflectionBasedModel
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
        var json = JsonSerializer.Serialize(originalModel, options);
        var deserializedModel = JsonSerializer.Deserialize<ReflectionBasedModel>(json, options);

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
    public void SourceGenerator_WithResolver_WithFormatAttribute_UsingOptions()
    {
        // Arrange
        var sourceGenOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default)
        };
        var originalModel = new SourceGeneratorWithResolverFormatModel
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
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorWithResolverFormatModel>(json, sourceGenOptions);

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
        var originalModel = new SourceGeneratorWithResolverFormatModel
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
        var deserializedModel = JsonSerializer.Deserialize<SourceGeneratorWithResolverFormatModel>(json, sourceGenOptions);

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
    public void SourceGenerator_WithResolver_WithFormatAttribute_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorWithResolverFormatModel);
        var context = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default);
        var originalModel = new SourceGeneratorWithResolverFormatModel
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
        var json = JsonSerializer.Serialize(originalModel, testModelType, context);
        var deserializedModel = (SourceGeneratorWithResolverFormatModel?)JsonSerializer.Deserialize(json, testModelType, context);

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
    public void SourceGenerator_WithResolver_WithFormatAttribute_WithNullValues_UsingContext()
    {
        // Arrange
        var testModelType = typeof(SourceGeneratorWithResolverFormatModel);
        var context = new DateTimeConverterResolver(ResolverModelJsonSerializerContext.Default);
        var originalModel = new SourceGeneratorWithResolverFormatModel
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
        var deserializedModel = (SourceGeneratorWithResolverFormatModel?)JsonSerializer.Deserialize(json, testModelType, context);

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
}