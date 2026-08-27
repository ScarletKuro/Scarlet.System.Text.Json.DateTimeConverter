using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

public class DateTimeConverterResolverTests
{
    [Fact]
    public void DateTimeConverterResolver_ParameterlessConstructor_CanBeCreated()
    {
        // Arrange & Act
        var resolver = new DateTimeConverterResolver();

        // Assert
        Assert.NotNull(resolver);
    }

    [Fact]
    public void DateTimeConverterResolver_WithJsonSerializerOptionsConstructor_CanBeCreated()
    {
        // Arrange
        var options = new JsonSerializerOptions();

        // Act
        var resolver = new DateTimeConverterResolver(options);

        // Assert
        Assert.NotNull(resolver);
    }

    [Fact]
    public void DateTimeConverterResolver_ParameterlessConstructor_GetTypeInfo_ReturnsNull()
    {
        // Arrange
        var resolver = new DateTimeConverterResolver();

        // Act
        // This exercises the parameterless constructor and the fallback path in GetTypeInfo(Type)
        // since _source is null, it calls GetTypeInfo(type, Options), which returns null
        var typeInfo = resolver.GetTypeInfo(typeof(SourceGeneratorModelWithAttribute));

        // Assert
        // Without a source resolver, GetTypeInfo should return null
        Assert.Null(typeInfo);
    }

    [Fact]
    public void DateTimeConverterResolver_WithOptionsConstructor_GetTypeInfo_UsesFallbackPath_ReturnsNull()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = ResolverModelJsonSerializerContext.Default
        };
        // Create a resolver with options - this exercises the JsonSerializerOptions constructor
        var resolver = new DateTimeConverterResolver(options);

        // Act
        // Call the single-parameter GetTypeInfo
        // This will use the fallback path: GetTypeInfo(type, Options) on line 59
        // since _source is null (not a JsonSerializerContext)
        // However, GetTypeInfo(Type, JsonSerializerOptions) needs _source to work, so it returns null
        var typeInfo = resolver.GetTypeInfo(typeof(SourceGeneratorModelWithAttribute));

        // Assert
        // Without _source set, the resolver can't resolve type info
        Assert.Null(typeInfo);
    }

    [Fact]
    public void DateTimeConverterResolver_GetTypeInfo_WithJsonSerializerContextSource_UsesContextOptions()
    {
        // Arrange
        var sourceContext = ResolverModelJsonSerializerContext.Default;
        var resolver = new DateTimeConverterResolver(sourceContext);

        // Act
        // This should use the JsonSerializerContext path in GetTypeInfo(Type)
        // where it detects _source is JsonSerializerContext and uses its Options
        var typeInfo = resolver.GetTypeInfo(typeof(SourceGeneratorModelWithAttribute));

        // Assert
        Assert.NotNull(typeInfo);
        Assert.Equal(typeof(SourceGeneratorModelWithAttribute), typeInfo.Type);
    }

    [Fact]
    public void DateTimeConverterResolver_WithParameterlessConstructor_CanSerializeWithSourceContext()
    {
        // Arrange
        var resolver = new DateTimeConverterResolver();
        var options = new JsonSerializerOptions
        {
            TypeInfoResolver = JsonTypeInfoResolver.Combine(resolver, ResolverModelJsonSerializerContext.Default),
            WriteIndented = true
        };
        var model = new SourceGeneratorModelWithAttribute
        {
            DateTimeProperty = new DateTime(2023, 10, 1, 12, 0, 0, DateTimeKind.Utc),
            DateOnlyProperty = new DateOnly(2023, 10, 1)
        };

        // Act
        var json = JsonSerializer.Serialize(model, options);

        // Assert
        // Should use default formats since resolver has no source with attributes
        Assert.Contains("\"DateTimeProperty\": \"2023-10-01T12:00:00Z\"", json);
        Assert.Contains("\"DateOnlyProperty\": \"2023-10-01\"", json);
    }

    [Fact]
    public void DateTimeConverterResolver_WithOptionsConstructor_StoresOptionsForBaseClass()
    {
        // Arrange
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            TypeInfoResolver = ResolverModelJsonSerializerContext.Default
        };
        // This tests the constructor with JsonSerializerOptions parameter
        // The options are passed to the base class JsonSerializerContext
        var resolver = new DateTimeConverterResolver(options);

        // Act & Assert
        // The resolver is successfully created with the options
        // The constructor is used when the resolver is part of framework integration
        Assert.NotNull(resolver);
    }

    [Fact]
    public void DateTimeConverterResolver_WithIJsonTypeInfoResolverSource_UsesSourceForGetTypeInfo()
    {
        // Arrange
        var sourceResolver = ResolverModelJsonSerializerContext.Default;
        var resolver = new DateTimeConverterResolver(sourceResolver);
        var options = new JsonSerializerOptions();

        // Act
        // Call GetTypeInfo with explicit options - this tests the two-parameter GetTypeInfo
        var typeInfo = resolver.GetTypeInfo(typeof(SourceGeneratorModelWithAttribute), options);

        // Assert
        // Should get typeInfo from the source resolver
        Assert.NotNull(typeInfo);
        Assert.Equal(typeof(SourceGeneratorModelWithAttribute), typeInfo.Type);
    }
}

