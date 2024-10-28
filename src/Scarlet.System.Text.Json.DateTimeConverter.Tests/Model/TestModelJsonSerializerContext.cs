using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

[JsonSerializable(typeof(TestModel))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public sealed partial class TestModelJsonSerializerContext : JsonSerializerContext;