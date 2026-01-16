# Scarlet.System.Text.Json.DateTimeConverter

[![Nuget](https://img.shields.io/nuget/v/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![Nuget](https://img.shields.io/nuget/dt/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&label=nuget%20downloads&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![GitHub](https://img.shields.io/github/license/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter?color=594ae2&logo=github)](https://github.com/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter/blob/master/LICENSE)
[![codecov](https://codecov.io/gh/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter/graph/badge.svg?token=6BNZEMSA7X)](https://codecov.io/gh/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter)

A flexible and powerful library for customizing `DateTime`, `DateTimeOffset`, `DateOnly`, and `TimeOnly` serialization in System.Text.Json, with full support for both reflection-based and source generator approaches.

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

This package provides three ways to specify custom date formats for `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, and their nullable counterparts when serializing and deserializing JSON using `System.Text.Json`:

1. **`JsonDateTimeConverterAttribute`** - Simple attribute-based approach for reflection-based serialization, or for System.Text.Json source generator with .NET 9+ using `DateTimeConverterResolver` (produces warnings)
2. **`JsonDateTimeFormatAttribute` + `DateTimeConverterResolver`** - Clean attribute-based approach for System.Text.Json source generator with .NET 9+ (no warnings)
3. **`JsonDateTimeFormatConverter<T>`** - Type-safe converter that works with both reflection-based serialization and System.Text.Json source generator (all .NET versions)

## Installation

```bash
dotnet add package Scarlet.System.Text.Json.DateTimeConverter
```

## Prerequisites

- **.NET 6+** for basic functionality
- **.NET 9+** for `DateTimeConverterResolver` (System.Text.Json source generator attribute support)

| Target Framework | Reflection + Attribute | System.Text.Json Source Generator + Converter | System.Text.Json Source Generator + Attribute + Resolver |
|-----------------|:---------------------:|:------------------------------------------:|:------------------------------------------------------:|
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

**Best for System.Text.Json source generator** (.NET 9+, no warnings):

```csharp
public class MyModel
{
    [JsonDateTimeFormat("yyyy-MM-dd")]
    public DateTime Date { get; set; }
}

[JsonSerializable(typeof(MyModel))]
public partial class MyJsonContext : JsonSerializerContext { }

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DateTimeConverterResolver(MyJsonContext.Default)
};
var json = JsonSerializer.Serialize(new MyModel { Date = DateTime.Now }, options);
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

    [JsonDateTimeConverter("MM/dd/yyyy")]
    public DateOnly DeliveryDate { get; set; }

    [JsonDateTimeConverter("HH:mm")]
    public TimeOnly DeliveryTime { get; set; }
}

// Usage
var order = new Order
{
    OrderDate = new DateTime(2026, 1, 15),
    ProcessedDate = new DateTime(2026, 1, 15, 14, 30, 0),
    ShippedAt = DateTimeOffset.UtcNow,
    DeliveryDate = new DateOnly(2026, 1, 20),
    DeliveryTime = new TimeOnly(10, 30)
};

string json = JsonSerializer.Serialize(order);
Console.WriteLine(json);
// Output: {"OrderDate":"2026-01-15","ProcessedDate":"2026-01-15T14:30:00","ShippedAt":"2026-01-15T14:30:00.123Z","DeliveryDate":"01/20/2026","DeliveryTime":"10:30"}

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

Use `JsonDateTimeFormatConverter<T>` for compatibility with System.Text.Json source generator across all .NET versions. This approach also works with reflection-based serialization.

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

    [JsonConverter(typeof(JsonDateTimeFormatConverter<DateFormats.DateOnlySlash>))]
    public DateOnly DeliveryDate { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<DateFormats.TimeOnlyShort>))]
    public TimeOnly DeliveryTime { get; set; }
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

    public class DateOnlySlash : IJsonDateTimeFormat
    {
        public static string Format => "MM/dd/yyyy";
    }

    public class TimeOnlyShort : IJsonDateTimeFormat
    {
        public static string Format => "HH:mm";
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
    ShippedAt = DateTimeOffset.UtcNow,
    DeliveryDate = new DateOnly(2026, 1, 20),
    DeliveryTime = new TimeOnly(10, 30)
};

string json = JsonSerializer.Serialize(order, typeof(Order), OrderJsonContext.Default);
Console.WriteLine(json);

var deserializedOrder = (Order?)JsonSerializer.Deserialize(json, typeof(Order), OrderJsonContext.Default);
```

**✅ Pros:**
- Works with System.Text.Json source generator (AOT-friendly)
- Also works with reflection-based serialization
- Compatible with all .NET versions (6+)
- Type-safe format definitions
- Does not require `DateTimeConverterResolver`

**❌ Cons:**
- Requires defining a class for each date format
- More verbose than attribute-based approach
- Format classes add boilerplate code

---

### Source Generator with Resolver (.NET 9+)

**.NET 9+** populates `JsonPropertyInfo.AttributeProvider` in System.Text.Json source generator, enabling attribute-based syntax with `DateTimeConverterResolver`.

#### Option A: JsonDateTimeFormatAttribute (Recommended - No Warnings)

Use `JsonDateTimeFormatAttribute` for the cleanest experience without SYSLIB1223 warnings:

```csharp
using Scarlet.System.Text.Json.DateTimeConverter;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Order
{
    [JsonDateTimeFormat("yyyy-MM-dd")]
    public DateTime OrderDate { get; set; }

    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss")]
    public DateTime? ProcessedDate { get; set; }

    [JsonDateTimeFormat("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset ShippedAt { get; set; }

    [JsonDateTimeFormat("MM/dd/yyyy")]
    public DateOnly DeliveryDate { get; set; }

    [JsonDateTimeFormat("HH:mm")]
    public TimeOnly DeliveryTime { get; set; }
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
- Clean attribute syntax with System.Text.Json source generator
- AOT-friendly
- **No SYSLIB1223 warnings**
- Best of both worlds: readability + performance

**❌ Cons:**
- **Requires .NET 9+**
- Requires using `DateTimeConverterResolver` to wrap the context
- Slightly more setup (need to wrap context with resolver)

#### Option B: JsonDateTimeConverterAttribute (Backward Compatible - Has Warnings)

You can also use `JsonDateTimeConverterAttribute` with `DateTimeConverterResolver` (for backward compatibility), but it will produce SYSLIB1223 warnings:

```csharp
public class Order
{
    [JsonDateTimeConverter("yyyy-MM-dd")]  // ⚠️ Produces SYSLIB1223 warning
    public DateTime OrderDate { get; set; }
}
```

The resolver still works, but the System.Text.Json source generator will emit warnings because `JsonDateTimeConverterAttribute` derives from `JsonConverterAttribute`.

**✅ Pros:**
- Works with existing code using `JsonDateTimeConverterAttribute`
- Backward compatible

**❌ Cons:**
- Produces SYSLIB1223 warnings during build
- May confuse users about the warnings

---

## Available Components

### `JsonDateTimeConverterAttribute`

A `JsonConverterAttribute`-derived attribute for specifying date formats directly on properties.

```csharp
[JsonDateTimeConverter("yyyy-MM-dd")]
public DateTime Date { get; set; }
```

**When to use:** Reflection-based serialization. Can also be used with .NET 9+ System.Text.Json source generator using `DateTimeConverterResolver`, but produces SYSLIB1223 warnings.

---

### `JsonDateTimeFormatAttribute` (.NET 9+)

A simple `Attribute`-derived attribute for specifying date formats with System.Text.Json source generator (no warnings). Must be used with `DateTimeConverterResolver`.

```csharp
[JsonDateTimeFormat("yyyy-MM-dd")]
public DateTime Date { get; set; }
```

**When to use:** .NET 9+ System.Text.Json source generator with `DateTimeConverterResolver` (recommended, no warnings).

---

### `JsonDateTimeFormatConverter<T>`

A `JsonConverterFactory` that uses `IJsonDateTimeFormat` implementations to define formats. Works with both reflection-based serialization and System.Text.Json source generator.

```csharp
public class MyFormat : IJsonDateTimeFormat
{
    public static string Format => "yyyy-MM-dd";
}

[JsonConverter(typeof(JsonDateTimeFormatConverter<MyFormat>))]
public DateTime Date { get; set; }
```

**When to use:** System.Text.Json source generator on any .NET version (6+), or reflection-based serialization when you want type-safe format definitions.

---

### `DateTimeConverterResolver` (.NET 9+)

A `JsonSerializerContext` and `IJsonTypeInfoResolver` that enables attribute-based date formatting with System.Text.Json source generator by using contract customization. Required when using `JsonDateTimeFormatAttribute`.

```csharp
var resolver = new DateTimeConverterResolver(MyJsonContext.Default);
var options = new JsonSerializerOptions { TypeInfoResolver = resolver };
```

**When to use:** .NET 9+ System.Text.Json source generator with `JsonDateTimeFormatAttribute` or `JsonDateTimeConverterAttribute`.

---

## When to Use What

| Scenario | Recommended Approach |
|----------|---------------------|
| Reflection-based, any .NET version | `JsonDateTimeConverterAttribute` |
| System.Text.Json source generator, .NET 6-8 | `JsonDateTimeFormatConverter<T>` |
| System.Text.Json source generator, .NET 9+ (no warnings) | `JsonDateTimeFormatAttribute` + `DateTimeConverterResolver` |
| System.Text.Json source generator, .NET 9+ (backward compat) | `JsonDateTimeConverterAttribute` + `DateTimeConverterResolver` (⚠️ warnings) |
| Need reusable formats across many properties | `JsonDateTimeFormatConverter<T>` (define format class once) |
| Prototyping/simple projects | `JsonDateTimeConverterAttribute` (simplest) |
| AOT compilation | `JsonDateTimeFormatConverter<T>` or .NET 9+ resolver with attributes |

---

## Quirks and Limitations

### Source Generator Limitations (.NET 6-8)

`JsonDateTimeConverterAttribute` produces **SYSLIB1223** warning with System.Text.Json source generator in .NET 6-8:

> "Attributes deriving from JsonConverterAttribute are not supported by the source generator."

**Solution:** Use `JsonDateTimeFormatConverter<T>` instead, or upgrade to .NET 9+ and use `JsonDateTimeFormatAttribute` with `DateTimeConverterResolver` (no warnings).

---

### SYSLIB1223 Warning with JsonDateTimeConverterAttribute (.NET 9+ Source Generators)

When using `JsonDateTimeConverterAttribute` with System.Text.Json source generator in .NET 9+, you'll get SYSLIB1223 warnings:

> "Attributes deriving from JsonConverterAttribute are not supported by the source generator."

This happens because `JsonDateTimeConverterAttribute` derives from `JsonConverterAttribute`. The code still works with `DateTimeConverterResolver`, but the warnings may be confusing.

**Solution:** Use `JsonDateTimeFormatAttribute` instead, which derives only from `Attribute` and produces no warnings:

```csharp
// ❌ Produces warnings (but still works)
[JsonDateTimeConverter("yyyy-MM-dd")]
public DateTime Date { get; set; }

// ✅ No warnings
[JsonDateTimeFormat("yyyy-MM-dd")]
public DateTime Date { get; set; }
```

---

### Format Class Per Format

With `JsonDateTimeFormatConverter<T>`, you need one class per unique format:

```csharp
public class Format1 : IJsonDateTimeFormat { public static string Format => "yyyy-MM-dd"; }
public class Format2 : IJsonDateTimeFormat { public static string Format => "yyyy-MM-ddTHH:mm:ss"; }
```

This is a limitation of System.Text.Json source generator not supporting constructor parameters or static analyzer tricks.

---

### .NET 9+ Resolver Requirement

`DateTimeConverterResolver` **only works on .NET 9+** because, while `JsonPropertyInfo.AttributeProvider` exists in .NET 7-8, it is not populated by System.Text.Json source generator until .NET 9+. See [runtime#100095](https://github.com/dotnet/runtime/issues/100095) and [runtime#102078](https://github.com/dotnet/runtime/issues/102078) for details.

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
- `DateOnly`
- `DateOnly?`
- `TimeOnly`
- `TimeOnly?`

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
