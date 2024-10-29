using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

[JsonSerializable(typeof(TestModelSourceGenerator))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class TestModelSourceGeneratorJsonSerializerContext : JsonSerializerContext;