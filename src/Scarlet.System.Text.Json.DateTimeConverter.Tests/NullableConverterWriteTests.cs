using System.Text;
using System.Text.Json;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Direct unit tests for nullable converters' Write method to ensure WriteNullValue() path is covered.
/// These tests directly invoke the converter's Write method with null values.
/// </summary>
public class NullableConverterWriteTests
{
    private const string TestDateFormat = "yyyy-MM-dd";
    private const string TestTimeFormat = "HH:mm:ss";
    private const int TestYear = 2023;
    private const int TestMonth = 10;
    private const int TestDay = 15;
    private const int TestHour = 14;
    private const int TestMinute = 30;
    private const int TestSecond = 45;

    [Fact]
    public void DateTimeNullableConverter_Write_DirectCallWithNull_WritesNullValue()
    {
        // Arrange
        var converter = Converters.DateTimeNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTime? value = null;

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateTimeNullableConverter_Write_DirectCallWithValue_WritesFormattedDate()
    {
        // Arrange
        var converter = Converters.DateTimeNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTime? value = new DateTime(TestYear, TestMonth, TestDay);

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("\"2023-10-15\"", json);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_DirectCallWithNull_WritesNullValue()
    {
        // Arrange
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTimeOffset? value = null;

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_DirectCallWithValue_WritesFormattedDate()
    {
        // Arrange
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTimeOffset? value = new DateTimeOffset(TestYear, TestMonth, TestDay, 0, 0, 0, TimeSpan.Zero);

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("\"2023-10-15\"", json);
    }

    [Fact]
    public void DateOnlyNullableConverter_Write_DirectCallWithNull_WritesNullValue()
    {
        // Arrange
        var converter = Converters.DateOnlyNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateOnly? value = null;

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("null", json);
    }

    [Fact]
    public void DateOnlyNullableConverter_Write_DirectCallWithValue_WritesFormattedDate()
    {
        // Arrange
        var converter = Converters.DateOnlyNullableConverter.FromFormat(TestDateFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateOnly? value = new DateOnly(TestYear, TestMonth, TestDay);

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("\"2023-10-15\"", json);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Write_DirectCallWithNull_WritesNullValue()
    {
        // Arrange
        var converter = Converters.TimeOnlyNullableConverter.FromFormat(TestTimeFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        TimeOnly? value = null;

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("null", json);
    }

    [Fact]
    public void TimeOnlyNullableConverter_Write_DirectCallWithValue_WritesFormattedTime()
    {
        // Arrange
        var converter = Converters.TimeOnlyNullableConverter.FromFormat(TestTimeFormat);
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        TimeOnly? value = new TimeOnly(TestHour, TestMinute, TestSecond);

        // Act
        converter.Write(writer, value, new JsonSerializerOptions());
        writer.Flush();

        // Assert
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Equal("\"14:30:45\"", json);
    }

    [Fact]
    public void DateTimeNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.DateTimeNullableConverter.FromFormat(TestDateFormat);
        DateTime? value = new DateTime(TestYear, TestMonth, TestDay);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat(TestDateFormat);
        DateTimeOffset? value = new DateTimeOffset(TestYear, TestMonth, TestDay, 0, 0, 0, TimeSpan.Zero);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void DateOnlyNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.DateOnlyNullableConverter.FromFormat(TestDateFormat);
        DateOnly? value = new DateOnly(TestYear, TestMonth, TestDay);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void TimeOnlyNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.TimeOnlyNullableConverter.FromFormat(TestTimeFormat);
        TimeOnly? value = new TimeOnly(TestHour, TestMinute, TestSecond);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }
}
