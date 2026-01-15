using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

[JsonSerializable(typeof(TestModelSourceGenerator))]
[JsonSerializable(typeof(TestModelSourceGeneratorAttributes))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class TestModelSourceGeneratorJsonSerializerContext : JsonSerializerContext;