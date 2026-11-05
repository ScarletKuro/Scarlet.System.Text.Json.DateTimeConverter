using System.Text.Json.Serialization;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

/// <summary>
/// JsonSerializerContext for the supported date/time types, used by converter tests that serialize them directly.
/// </summary>
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(DateTime?))]
[JsonSerializable(typeof(DateTimeOffset))]
[JsonSerializable(typeof(DateTimeOffset?))]
[JsonSerializable(typeof(DateOnly))]
[JsonSerializable(typeof(DateOnly?))]
[JsonSerializable(typeof(TimeOnly))]
[JsonSerializable(typeof(TimeOnly?))]
internal sealed partial class PrimitiveJsonSerializerContext : JsonSerializerContext;
