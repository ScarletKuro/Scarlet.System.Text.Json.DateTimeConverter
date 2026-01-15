# Scarlet.System.Text.Json.DateTimeConverter

[![Nuget](https://img.shields.io/nuget/v/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![Nuget](https://img.shields.io/nuget/dt/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&label=nuget%20downloads&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![GitHub](https://img.shields.io/github/license/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter?color=594ae2&logo=github)](https://github.com/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter/blob/master/LICENSE)

A flexible and powerful library for customizing `DateTime` and `DateTimeOffset` serialization in System.Text.Json, with full support for both reflection-based and source generator approaches.

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Usage Scenarios](#usage-scenarios)
  - [Reflection-Based Serialization (.NET 6+)](#reflection-based-serialization-net-6)
  - [Source Generator with Format Converter (.NET 6+)](#source-generator-with-format-converter-net-6)
  - [Source Generator with Attribute and Resolver (.NET 9+)](#source-generator-with-attribute-and-resolver-net-9)
- [Available Components](#available-components)
- [When to Use What](#when-to-use-what)
- [Quirks and Limitations](#quirks-and-limitations)
- [Supported Types](#supported-types)
- [License](#license)

## Overview

This package provides three ways to specify custom date formats for `DateTime`, `DateTimeOffset`, and their nullable counterparts when serializing and deserializing JSON using `System.Text.Json`:

1. **`JsonDateTimeConverterAttribute`** - Simple attribute-based approach (reflection only, or .NET 9+ with resolver)
2. **`JsonDateTimeFormatConverter<T>`** - Type-safe converter for source generators
3. **`DateTimeConverterResolver`** - Contract customization for .NET 9+ source generators

## Installation

```bash
dotnet add package Scarlet.System.Text.Json.DateTimeConverter
```

## Prerequisites

- **.NET 6+** for basic functionality
- **.NET 9+** for `DateTimeConverterResolver` (source generator attribute support)

| Target Framework | Reflection + Attribute | Source Generator + Converter | Source Generator + Attribute + Resolver |
|-----------------|:---------------------:|:---------------------------:|:--------------------------------------:|
| .NET 6, 7, 8    | ✅ | ✅ | ❌ |
| .NET 9, 10+     | ✅ | ✅ | ✅ |

## Quick Start

**Simplest approach** (reflection-based):

```csharp
public class MyModel
{
    [JsonDateTimeConverter("yyyy-MM-dd")]
    public DateTime Date { get; set; }
}

var json = JsonSerializer.Serialize(new MyModel { Date = DateTime.Now });
// Output: {"Date":"2026-01-15"}
```

## Usage Scenarios

### Reflection-Based Serialization (.NET 6+)

Use `JsonDateTimeConverterAttribute` for the simplest, most readable approach with reflection-based serialization.

```csharp
using Scarlet.System.Text.Json.DateTimeConverter;
using System.Text.Json;

public class Order
{
    [JsonDateTimeConverter("yyyy-MM-dd")]
    public DateTime OrderDate { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? ProcessedDate { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset ShippedAt { get; set; }
}

// Usage
var order = new Order
{
    OrderDate = new DateTime(2026, 1, 15),
    ProcessedDate = new DateTime(2026, 1, 15, 14, 30, 0),
    ShippedAt = DateTimeOffset.UtcNow
};

string json = JsonSerializer.Serialize(order);
Console.WriteLine(json);
// Output: {"OrderDate":"2026-01-15","ProcessedDate":"2026-01-15T14:30:00","ShippedAt":"2026-01-15T14:30:00.123Z"}

var deserializedOrder = JsonSerializer.Deserialize<Order>(json);
```

**✅ Pros:**
- Clean, readable code with attribute decoration
- Works with all .NET versions (6+)
- Easy to use and understand

**❌ Cons:**
- Only works with reflection-based serialization
- Produces SYSLIB1223 warning with source generators (.NET 6-8)
- No AOT (Ahead-of-Time) compilation support

---

### Source Generator with Format Converter (.NET 6+)

Use `JsonDateTimeFormatConverter<T>` for source generator compatibility across all .NET versions.

```csharp
using Scarlet.System.Text.Json.DateTimeConverter;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Order
{
    [JsonConverter(typeof(JsonDateTimeFormatConverter<DateFormats.DateOnly>))]
    public DateTime OrderDate { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<DateFormats.DateTimeSeconds>))]
    public DateTime? ProcessedDate { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<DateFormats.ISO8601>))]
    public DateTimeOffset ShippedAt { get; set; }
}

// Define your custom date formats
public static class DateFormats
{
    public class DateOnly : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-dd";
    }

    public class DateTimeSeconds : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss";
    }

    public class ISO8601 : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss.fffZ";
    }
}

// Create a JsonSerializerContext for source generation
[JsonSerializable(typeof(Order))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class OrderJsonContext : JsonSerializerContext { }

// Usage with source generator
var order = new Order
{
    OrderDate = new DateTime(2026, 1, 15),
    ProcessedDate = new DateTime(2026, 1, 15, 14, 30, 0),
    ShippedAt = DateTimeOffset.UtcNow
};

string json = JsonSerializer.Serialize(order, typeof(Order), OrderJsonContext.Default);
Console.WriteLine(json);

var deserializedOrder = (Order?)JsonSerializer.Deserialize(json, typeof(Order), OrderJsonContext.Default);
```

**✅ Pros:**
- Works with source generators (AOT-friendly)
- Compatible with all .NET versions (6+)
- Type-safe format definitions

**❌ Cons:**
- Requires defining a class for each date format
- More verbose than attribute-based approach
- Format classes add boilerplate code

---

### Source Generator with Attribute and Resolver (.NET 9+)

**.NET 9+** populates `JsonPropertyInfo.AttributeProvider` in source generators, enabling attribute-based syntax with `DateTimeConverterResolver`.

```csharp
using Scarlet.System.Text.Json.DateTimeConverter;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Order
{
    [JsonDateTimeConverter("yyyy-MM-dd")]
    public DateTime OrderDate { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? ProcessedDate { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset ShippedAt { get; set; }
}

[JsonSerializable(typeof(Order))]
[JsonSourceGenerationOptions(WriteIndented = true)]
public partial class OrderJsonContext : JsonSerializerContext { }

// Usage - Method 1: With JsonSerializerOptions
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new DateTimeConverterResolver(OrderJsonContext.Default)
};

string json = JsonSerializer.Serialize(order, options);
var deserializedOrder = JsonSerializer.Deserialize<Order>(json, options);

// Usage - Method 2: With Context directly
var resolver = new DateTimeConverterResolver(OrderJsonContext.Default);
string json = JsonSerializer.Serialize(order, typeof(Order), resolver);
var deserializedOrder = (Order?)JsonSerializer.Deserialize(json, typeof(Order), resolver);
```

**✅ Pros:**
- Clean attribute syntax with source generators
- AOT-friendly
- Best of both worlds: readability + performance

**❌ Cons:**
- **Requires .NET 9+**
- Slightly more setup (need to wrap context with resolver)

---

## Available Components

### `JsonDateTimeConverterAttribute`

A `JsonConverterAttribute`-derived attribute for specifying date formats directly on properties.

```csharp
[JsonDateTimeConverter("yyyy-MM-dd")]
public DateTime Date { get; set; }
```

**When to use:** Reflection-based serialization, or .NET 9+ with `DateTimeConverterResolver`.

---

### `JsonDateTimeFormatConverter<T>`

A `JsonConverterFactory` that uses `IJsonDateTimeFormat` implementations to define formats.

```csharp
public class MyFormat : IJsonDateTimeFormat
{
    public static string Format => "yyyy-MM-dd";
}

[JsonConverter(typeof(JsonDateTimeFormatConverter<MyFormat>))]
public DateTime Date { get; set; }
```

**When to use:** Source generators on any .NET version (6+).

---

### `DateTimeConverterResolver` (.NET 9+)

A `JsonSerializerContext` and `IJsonTypeInfoResolver` that enables `JsonDateTimeConverterAttribute` to work with source generators by using contract customization.

```csharp
var resolver = new DateTimeConverterResolver(MyJsonContext.Default);
var options = new JsonSerializerOptions { TypeInfoResolver = resolver };
```

**When to use:** .NET 9+ source generators with attribute syntax.

---

## When to Use What

| Scenario | Recommended Approach |
|----------|---------------------|
| Reflection-based, any .NET version | `JsonDateTimeConverterAttribute` |
| Source generator, .NET 6-8 | `JsonDateTimeFormatConverter<T>` |
| Source generator, .NET 9+ | `JsonDateTimeConverterAttribute` + `DateTimeConverterResolver` |
| Need reusable formats across many properties | `JsonDateTimeFormatConverter<T>` (define format class once) |
| Prototyping/simple projects | `JsonDateTimeConverterAttribute` (simplest) |
| AOT compilation | `JsonDateTimeFormatConverter<T>` or .NET 9+ resolver |

---

## Quirks and Limitations

### Source Generator Limitations (.NET 6-8)

`JsonDateTimeConverterAttribute` produces **SYSLIB1223** warning with source generators in .NET 6-8:

> "Attributes deriving from JsonConverterAttribute are not supported by the source generator."

**Solution:** Use `JsonDateTimeFormatConverter<T>` instead, or upgrade to .NET 9+.

---

### Format Class Per Format

With `JsonDateTimeFormatConverter<T>`, you need one class per unique format:

```csharp
public class Format1 : IJsonDateTimeFormat { public static string Format => "yyyy-MM-dd"; }
public class Format2 : IJsonDateTimeFormat { public static string Format => "yyyy-MM-ddTHH:mm:ss"; }
```

This is a limitation of source generators not supporting constructor parameters or static analyzer tricks.

---

### .NET 9+ Resolver Requirement

`DateTimeConverterResolver` **only works on .NET 9+** because, while `JsonPropertyInfo.AttributeProvider` exists in .NET 7-8, it is not populated by source generators until .NET 9+. See [runtime#100095](https://github.com/dotnet/runtime/issues/100095) and [runtime#102078](https://github.com/dotnet/runtime/issues/102078) for details.

---

### Null Handling

Nullable types (`DateTime?`, `DateTimeOffset?`) write `null` in JSON when the value is `null`:

```json
{
  "NullableDate": null
}
```

This matches standard `System.Text.Json` behavior.

---

## Supported Types

- `DateTime`
- `DateTime?`
- `DateTimeOffset`
- `DateTimeOffset?`

All types support any valid [.NET date and time format string](https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Contributing

Contributions are welcome! Please open an issue or pull request on [GitHub](https://github.com/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter).

---

## Support

If you encounter any issues or have questions, please open an issue on the [GitHub repository](https://github.com/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter/issues).
