using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Unit tests for DateTimeConverterFactoryHelper to ensure full code coverage.
/// </summary>
public class DateTimeConverterFactoryHelperTests
{
    [Fact]
    public void CreateConverter_DateTime_ReturnsDateTimeConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateTime), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateTimeConverter>(converter);
    }

    [Fact]
    public void CreateConverter_NullableDateTime_ReturnsDateTimeNullableConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateTime?), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateTimeNullableConverter>(converter);
    }

    [Fact]
    public void CreateConverter_DateTimeOffset_ReturnsDateTimeOffsetConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateTimeOffset), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateTimeOffsetConverter>(converter);
    }

    [Fact]
    public void CreateConverter_NullableDateTimeOffset_ReturnsDateTimeOffsetNullableConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateTimeOffset?), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateTimeOffsetNullableConverter>(converter);
    }

    [Fact]
    public void CreateConverter_DateOnly_ReturnsDateOnlyConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateOnly), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateOnlyConverter>(converter);
    }

    [Fact]
    public void CreateConverter_NullableDateOnly_ReturnsDateOnlyNullableConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(DateOnly?), "yyyy-MM-dd");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.DateOnlyNullableConverter>(converter);
    }

    [Fact]
    public void CreateConverter_TimeOnly_ReturnsTimeOnlyConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(TimeOnly), "HH:mm:ss");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.TimeOnlyConverter>(converter);
    }

    [Fact]
    public void CreateConverter_NullableTimeOnly_ReturnsTimeOnlyNullableConverter()
    {
        // Act
        var converter = DateTimeConverterFactoryHelper.CreateConverter(typeof(TimeOnly?), "HH:mm:ss");

        // Assert
        Assert.NotNull(converter);
        Assert.IsType<Converters.TimeOnlyNullableConverter>(converter);
    }

    [Fact]
    public void CreateConverter_UnsupportedType_ThrowsNotSupportedException()
    {
        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(() =>
            DateTimeConverterFactoryHelper.CreateConverter(typeof(string), "yyyy-MM-dd"));

        Assert.Contains("System.String", exception.Message);
        Assert.Contains("DateTimeConverterFactoryHelper", exception.Message);
    }

    [Fact]
    public void CreateConverter_AnotherUnsupportedType_ThrowsNotSupportedException()
    {
        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(() =>
            DateTimeConverterFactoryHelper.CreateConverter(typeof(int), "yyyy-MM-dd"));

        Assert.Contains("System.Int32", exception.Message);
        Assert.Contains("DateTimeConverterFactoryHelper", exception.Message);
    }

    [Fact]
    public void CreateConverter_NullType_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            DateTimeConverterFactoryHelper.CreateConverter(null!, "yyyy-MM-dd"));
    }
}
