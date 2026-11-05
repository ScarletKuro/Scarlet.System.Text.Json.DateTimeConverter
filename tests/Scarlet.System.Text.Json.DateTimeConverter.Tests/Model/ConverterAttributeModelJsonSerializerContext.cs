using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// JsonSerializerContext for the model using JsonDateTimeConverterAttribute with DateTimeConverterResolver.
/// </summary>
[JsonSerializable(typeof(SourceGeneratorModelWithConverterAttribute))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class ConverterAttributeModelJsonSerializerContext : JsonSerializerContext;
