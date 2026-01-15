using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// JsonSerializerContext for models using JsonConverter attribute with JsonDateTimeFormatConverter.
/// </summary>
[JsonSerializable(typeof(SourceGeneratorWithConverterModel))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class ConverterModelJsonSerializerContext : JsonSerializerContext;

/// <summary>
/// JsonSerializerContext for models using attributes with DateTimeConverterResolver.
/// </summary>
[JsonSerializable(typeof(SourceGeneratorWithResolverAttributeModel))]
[JsonSerializable(typeof(SourceGeneratorWithResolverFormatModel))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class ResolverModelJsonSerializerContext : JsonSerializerContext;
