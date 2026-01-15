using System.Text;
using System.Text.Json;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Direct unit tests for nullable converters' Write method to ensure WriteNullValue() path is covered.
/// These tests directly invoke the converter's Write method with null values.
/// </summary>
public class NullableConverterWriteTests
{
    [Fact]
    public void DateTimeNullableConverter_Write_DirectCallWithNull_WritesNullValue()
    {
        // Arrange
        var converter = Converters.DateTimeNullableConverter.FromFormat("yyyy-MM-dd");
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
        var converter = Converters.DateTimeNullableConverter.FromFormat("yyyy-MM-dd");
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTime? value = new DateTime(2023, 10, 15);

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
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat("yyyy-MM-dd");
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
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat("yyyy-MM-dd");
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateTimeOffset? value = new DateTimeOffset(2023, 10, 15, 0, 0, 0, TimeSpan.Zero);

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
        var converter = Converters.DateOnlyNullableConverter.FromFormat("yyyy-MM-dd");
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
        var converter = Converters.DateOnlyNullableConverter.FromFormat("yyyy-MM-dd");
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        DateOnly? value = new DateOnly(2023, 10, 15);

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
        var converter = Converters.TimeOnlyNullableConverter.FromFormat("HH:mm:ss");
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
        var converter = Converters.TimeOnlyNullableConverter.FromFormat("HH:mm:ss");
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream);
        TimeOnly? value = new TimeOnly(14, 30, 45);

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
        var converter = Converters.DateTimeNullableConverter.FromFormat("yyyy-MM-dd");
        DateTime? value = new DateTime(2023, 10, 15);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void DateTimeOffsetNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.DateTimeOffsetNullableConverter.FromFormat("yyyy-MM-dd");
        DateTimeOffset? value = new DateTimeOffset(2023, 10, 15, 0, 0, 0, TimeSpan.Zero);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void DateOnlyNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.DateOnlyNullableConverter.FromFormat("yyyy-MM-dd");
        DateOnly? value = new DateOnly(2023, 10, 15);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }

    [Fact]
    public void TimeOnlyNullableConverter_Write_NullWriter_ThrowsArgumentNullException()
    {
        // Arrange
        var converter = Converters.TimeOnlyNullableConverter.FromFormat("HH:mm:ss");
        TimeOnly? value = new TimeOnly(14, 30, 45);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => converter.Write(null!, value, new JsonSerializerOptions()));
    }
}
