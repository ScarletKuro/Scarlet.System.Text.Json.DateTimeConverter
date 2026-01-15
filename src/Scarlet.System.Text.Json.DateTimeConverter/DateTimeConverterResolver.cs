#if NET9_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Scarlet.System.Text.Json.DateTimeConverter;

public class DateTimeConverterResolver : JsonSerializerContext, IJsonTypeInfoResolver
{
    private readonly IJsonTypeInfoResolver? _source;

    public DateTimeConverterResolver(IJsonTypeInfoResolver source) : base(null)
    {
        _source = source;
    }

    public DateTimeConverterResolver() : base(null)
    {
    }

    public DateTimeConverterResolver(JsonSerializerOptions options) : base(options)
    {
    }

    public JsonTypeInfo? GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo? jsonTypeInfo = _source?.GetTypeInfo(type, options);

        if (jsonTypeInfo is null)
        {
            return null;
        }

        foreach (var jsonPropertyInfo in jsonTypeInfo.Properties)
        {
            // First check for JsonDateTimeFormatAttribute (preferred for source generators, no warnings)
            if (jsonPropertyInfo.AttributeProvider?.GetCustomAttributes(typeof(JsonDateTimeFormatAttribute), inherit: false) is [JsonDateTimeFormatAttribute formatAttr, ..])
            {
                jsonPropertyInfo.CustomConverter = DateTimeConverterFactoryHelper.CreateConverter(jsonPropertyInfo.PropertyType, formatAttr.Format);
            }
            // Fall back to JsonDateTimeConverterAttribute for backward compatibility
            else if (jsonPropertyInfo.AttributeProvider?.GetCustomAttributes(typeof(JsonDateTimeConverterAttribute), inherit: false) is [JsonDateTimeConverterAttribute converterAttr, ..])
            {
                jsonPropertyInfo.CustomConverter = converterAttr.CreateConverter(jsonPropertyInfo.PropertyType);
            }
        }

        return jsonTypeInfo;
    }

    public override JsonTypeInfo? GetTypeInfo(Type type)
    {
        // We are wrapping DateTimeConverterResolver(SourceGeneratorXXX.Default) so it makes sense to forward the Options from SourceGeneratorXXX
        if (_source is JsonSerializerContext jsonSerializerContext)
        {
            return GetTypeInfo(type, jsonSerializerContext.Options);
        }

        return GetTypeInfo(type, Options);
    }

    protected override JsonSerializerOptions? GeneratedSerializerOptions => null;
}
#endif
