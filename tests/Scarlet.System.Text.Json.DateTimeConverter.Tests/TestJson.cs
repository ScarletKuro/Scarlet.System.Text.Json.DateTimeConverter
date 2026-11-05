using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Scarlet.System.Text.Json.DateTimeConverter.Tests.Model;

namespace Scarlet.System.Text.Json.DateTimeConverter.Tests;

/// <summary>
/// Serializes through the <see cref="JsonTypeInfo{T}"/> resolved from the options, so the tests compile and run
/// under NativeAOT with serializer reflection disabled.
/// </summary>
/// <remarks>
/// Options that carry a resolver (a source-generated context or <see cref="DateTimeConverterResolver"/>) are used
/// as-is, which is equivalent to the <c>JsonSerializer.Serialize&lt;T&gt;(value, options)</c> overload.
/// Options without a resolver keep the reflection-based default resolver under JIT; under NativeAOT, where reflection
/// is disabled, they fall back to <see cref="PrimitiveJsonSerializerContext"/>. Either way, converters registered in
/// <see cref="JsonSerializerOptions.Converters"/> take precedence over the type's default metadata.
/// </remarks>
internal static class TestJson
{
    public static string Serialize<T>(T value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(value, GetTypeInfo<T>(options));

    public static T? Deserialize<T>(string json, JsonSerializerOptions options)
        => JsonSerializer.Deserialize(json, GetTypeInfo<T>(options));

    private static JsonTypeInfo<T> GetTypeInfo<T>(JsonSerializerOptions options)
    {
#if NATIVE_AOT_TESTS
        options.TypeInfoResolver ??= PrimitiveJsonSerializerContext.Default;
#else
        // What the reflection-based JsonSerializer overloads do: fill in DefaultJsonTypeInfoResolver when unset.
        options.MakeReadOnly(populateMissingResolver: true);
#endif

        return (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
    }
}
