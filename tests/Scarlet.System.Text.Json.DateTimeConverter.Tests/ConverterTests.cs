using System.Text.Json;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Comprehensive unit tests for all individual converters to improve code coverage.
/// Tests cover all branches including Read/Write methods, null handling, and error paths.
/// </summary>
public class ConverterTests
{
    #region DateTimeConverter Tests

    [Fact]
    public void DateTimeConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-dd").CreateConverter(typeof(DateTime)) }
        };
        var date = new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"2023-10-15\"", json);
    }

    [Fact]
    public void DateTimeConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateTime)) }
        };
        var json = "\"10/15/2023\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        // Assert
        Assert.Equal(new DateTime(2023, 10, 15), result);
    }

    [Fact]
    public void DateTimeConverter_Read_InvalidFormat_FallbackToGetDateTime()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd-MM-yyyy").CreateConverter(typeof(DateTime)) }
        };
        // ISO 8601 format that doesn't match our custom format
        var json = "\"2023-10-15T14:30:45Z\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        // Assert - Should fallback to reader.GetDateTime()
        Assert.Equal(new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc), result);
    }

    [Fact]
    public void DateTimeConverter_Read_WithTimeComponent_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss").CreateConverter(typeof(DateTime)) }
        };
        var json = "\"2023-10-15T14:30:45\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        // Assert
        Assert.Equal(new DateTime(2023, 10, 15, 14, 30, 45), result);
    }

    [Fact]
    public void DateTimeConverter_RoundTrip_PreservesValue()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fff").CreateConverter(typeof(DateTime)) }
        };
        var original = new DateTime(2023, 10, 15, 14, 30, 45, 123, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTime>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    #endregion

    #region DateTimeNullableConverter Tests

    [Fact]
    public void DateTimeNullableConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy/MM/dd").CreateConverter(typeof(DateTime?)) }
        };
        DateTime? date = new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"2023/10/15\"", json);
    }

    [Fact]
    public void DateTimeNullableConverter_Write_NullValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyyMMdd").CreateConverter(typeof(DateTime?)) }
        };
        DateTime? date = null;

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateTimeNullableConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd.MM.yyyy").CreateConverter(typeof(DateTime?)) }
        };
        var json = "\"15.10.2023\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateTime(2023, 10, 15), result.Value);
    }

    [Fact]
    public void DateTimeNullableConverter_Read_NullToken_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy.MM.dd").CreateConverter(typeof(DateTime?)) }
        };
        var json = "null";

        // Act
        var result = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeNullableConverter_Read_InvalidFormat_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM-dd-yyyy").CreateConverter(typeof(DateTime?)) }
        };
        var json = "\"invalid-date\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeNullableConverter_RoundTrip_WithValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss").CreateConverter(typeof(DateTime?)) }
        };
        DateTime? original = new DateTime(2023, 10, 15, 14, 30, 45, DateTimeKind.Utc);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void DateTimeNullableConverter_RoundTrip_WithNull_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd/MM/yyyy").CreateConverter(typeof(DateTime?)) }
        };
        DateTime? original = null;

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTime?>(json, options);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region DateTimeOffsetConverter Tests

    [Fact]
    public void DateTimeOffsetConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset)) }
        };
        var date = new DateTimeOffset(2023, 10, 15, 14, 30, 45, 123, TimeSpan.Zero);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"2023-10-15T14:30:45.123Z\"", json);
    }

    [Fact]
    public void DateTimeOffsetConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset)) }
        };
        var json = "\"2023-10-15T14:30:45.123Z\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert
        Assert.Equal(new DateTimeOffset(2023, 10, 15, 14, 30, 45, 123, TimeSpan.Zero), result);
    }

    [Fact]
    public void DateTimeOffsetConverter_Read_InvalidFormat_FallbackToGetDateTimeOffset()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyyMMdd").CreateConverter(typeof(DateTimeOffset)) }
        };
        // Standard ISO 8601 format
        var json = "\"2023-10-15T14:30:45+00:00\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert - Should fallback to reader.GetDateTimeOffset()
        Assert.Equal(new DateTimeOffset(2023, 10, 15, 14, 30, 45, TimeSpan.Zero), result);
    }

    [Fact]
    public void DateTimeOffsetConverter_RoundTrip_PreservesValue()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:sszzz").CreateConverter(typeof(DateTimeOffset)) }
        };
        var original = new DateTimeOffset(2023, 10, 15, 14, 30, 45, TimeSpan.FromHours(5));

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    #endregion

    #region DateTimeOffsetNullableConverter Tests

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset?)) }
        };
        DateTimeOffset? date = new DateTimeOffset(2023, 10, 15, 14, 30, 45, 123, TimeSpan.Zero);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"2023-10-15T14:30:45.123Z\"", json);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_NullValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy/MM/dd").CreateConverter(typeof(DateTimeOffset?)) }
        };
        DateTimeOffset? date = null;

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:ss.fffZ").CreateConverter(typeof(DateTimeOffset?)) }
        };
        var json = "\"2023-10-15T14:30:45.123Z\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateTimeOffset(2023, 10, 15, 14, 30, 45, 123, TimeSpan.Zero), result.Value);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Read_NullToken_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd-MM-yyyy").CreateConverter(typeof(DateTimeOffset?)) }
        };
        var json = "null";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Read_InvalidFormat_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateTimeOffset?)) }
        };
        var json = "\"invalid-date\"";

        // Act
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_RoundTrip_WithValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-ddTHH:mm:sszzz").CreateConverter(typeof(DateTimeOffset?)) }
        };
        DateTimeOffset? original = new DateTimeOffset(2023, 10, 15, 14, 30, 45, TimeSpan.FromHours(3));

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_RoundTrip_WithNull_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd.MM.yyyy").CreateConverter(typeof(DateTimeOffset?)) }
        };
        DateTimeOffset? original = null;

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateTimeOffset?>(json, options);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region DateOnlyConverter Tests

    [Fact]
    public void DateOnlyConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly)) }
        };
        var date = new DateOnly(2023, 10, 15);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"10/15/2023\"", json);
    }

    [Fact]
    public void DateOnlyConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly)) }
        };
        var json = "\"10/15/2023\"";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly>(json, options);

        // Assert
        Assert.Equal(new DateOnly(2023, 10, 15), result);
    }

    [Fact]
    public void DateOnlyConverter_Read_InvalidFormat_FallbackToGetDateTime()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly)) }
        };
        // Standard ISO 8601 format
        var json = "\"2023-10-15\"";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly>(json, options);

        // Assert - Should fallback to DateOnly.FromDateTime(reader.GetDateTime())
        Assert.Equal(new DateOnly(2023, 10, 15), result);
    }

    [Fact]
    public void DateOnlyConverter_Read_InvalidToken_ThrowsJsonException()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd/MM/yyyy").CreateConverter(typeof(DateOnly)) }
        };
        var json = "\"invalid-date-format\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<DateOnly>(json, options));
    }

    [Fact]
    public void DateOnlyConverter_RoundTrip_PreservesValue()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-dd").CreateConverter(typeof(DateOnly)) }
        };
        var original = new DateOnly(2023, 10, 15);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateOnly>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    #endregion

    #region DateOnlyNullableConverter Tests

    [Fact]
    public void DateOnlyNullableConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        DateOnly? date = new DateOnly(2023, 10, 15);

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"10/15/2023\"", json);
    }

    [Fact]
    public void DateOnlyNullableConverter_Write_NullValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd-MM-yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        DateOnly? date = null;

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateOnlyNullableConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM/dd/yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        var json = "\"10/15/2023\"";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new DateOnly(2023, 10, 15), result.Value);
    }

    [Fact]
    public void DateOnlyNullableConverter_Read_NullToken_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("dd.MM.yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        var json = "null";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateOnlyNullableConverter_Read_InvalidFormat_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("MM-dd-yyyy").CreateConverter(typeof(DateOnly?)) }
        };
        var json = "\"invalid-date\"";

        // Act
        var result = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void DateOnlyNullableConverter_RoundTrip_WithValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy-MM-dd").CreateConverter(typeof(DateOnly?)) }
        };
        DateOnly? original = new DateOnly(2023, 10, 15);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void DateOnlyNullableConverter_RoundTrip_WithNull_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("yyyy.MM.dd").CreateConverter(typeof(DateOnly?)) }
        };
        DateOnly? original = null;

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<DateOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    #endregion

    #region TimeOnlyConverter Tests

    [Fact]
    public void TimeOnlyConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss").CreateConverter(typeof(TimeOnly)) }
        };
        var time = new TimeOnly(14, 30, 45);

        // Act
        var json = JsonSerializer.Serialize(time, options);

        // Assert
        Assert.Equal("\"14:30:45\"", json);
    }

    [Fact]
    public void TimeOnlyConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss").CreateConverter(typeof(TimeOnly)) }
        };
        var json = "\"14:30:45\"";

        // Act
        var result = JsonSerializer.Deserialize<TimeOnly>(json, options);

        // Assert
        Assert.Equal(new TimeOnly(14, 30, 45), result);
    }

    [Fact]
    public void TimeOnlyConverter_Read_InvalidFormat_FallbackToGetDateTime()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH.mm").CreateConverter(typeof(TimeOnly)) }
        };
        // Full datetime that doesn't match our format
        var json = "\"2023-10-15T14:30:45Z\"";

        // Act
        var result = JsonSerializer.Deserialize<TimeOnly>(json, options);

        // Assert - Should fallback to TimeOnly.FromDateTime(reader.GetDateTime())
        Assert.Equal(new TimeOnly(14, 30, 45), result);
    }

    [Fact]
    public void TimeOnlyConverter_Read_InvalidToken_ThrowsJsonException()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss").CreateConverter(typeof(TimeOnly)) }
        };
        var json = "\"invalid-time-format\"";

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TimeOnly>(json, options));
    }

    [Fact]
    public void TimeOnlyConverter_RoundTrip_PreservesValue()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss.fff").CreateConverter(typeof(TimeOnly)) }
        };
        var original = new TimeOnly(14, 30, 45, 123);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<TimeOnly>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    #endregion

    #region TimeOnlyNullableConverter Tests

    [Fact]
    public void TimeOnlyNullableConverter_Write_ValidValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss").CreateConverter(typeof(TimeOnly?)) }
        };
        TimeOnly? time = new TimeOnly(14, 30, 45);

        // Act
        var json = JsonSerializer.Serialize(time, options);

        // Assert
        Assert.Equal("\"14:30:45\"", json);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Write_NullValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH-mm-ss").CreateConverter(typeof(TimeOnly?)) }
        };
        TimeOnly? time = null;

        // Act
        var json = JsonSerializer.Serialize(time, options);

        // Assert
        Assert.Equal("null", json);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Read_ValidFormat_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss").CreateConverter(typeof(TimeOnly?)) }
        };
        var json = "\"14:30:45\"";

        // Act
        var result = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(new TimeOnly(14, 30, 45), result.Value);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Read_NullToken_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH.mm.ss").CreateConverter(typeof(TimeOnly?)) }
        };
        var json = "null";

        // Act
        var result = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Read_InvalidFormat_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("mm:ss:HH").CreateConverter(typeof(TimeOnly?)) }
        };
        var json = "\"invalid-time\"";

        // Act
        var result = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void TimeOnlyNullableConverter_RoundTrip_WithValue_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm:ss.fff").CreateConverter(typeof(TimeOnly?)) }
        };
        TimeOnly? original = new TimeOnly(14, 30, 45, 123);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.Equal(original, result);
    }

    [Fact]
    public void TimeOnlyNullableConverter_RoundTrip_WithNull_Success()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonDateTimeConverterAttribute("HH:mm").CreateConverter(typeof(TimeOnly?)) }
        };
        TimeOnly? original = null;

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var result = JsonSerializer.Deserialize<TimeOnly?>(json, options);

        // Assert
        Assert.Null(result);
    }

    #endregion
}
