using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

[JsonSerializable(typeof(SourceGeneratorModel))]
[JsonSerializable(typeof(SourceGeneratorWithResolverModel))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class TestModelSourceGeneratorJsonSerializerContext : JsonSerializerContext;