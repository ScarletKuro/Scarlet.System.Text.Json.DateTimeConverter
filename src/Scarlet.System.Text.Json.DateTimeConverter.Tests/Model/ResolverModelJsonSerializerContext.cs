using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// JsonSerializerContext for models using attributes with DateTimeConverterResolver.
/// </summary>
[JsonSerializable(typeof(SourceGeneratorModelWithAttribute))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class ResolverModelJsonSerializerContext : JsonSerializerContext;