## Overview
[![Nuget](https://img.shields.io/nuget/v/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![Nuget](https://img.shields.io/nuget/dt/Scarlet.System.Text.Json.DateTimeConverter?color=ff4081&label=nuget%20downloads&logo=nuget)](https://www.nuget.org/packages/Scarlet.System.Text.Json.DateTimeConverter)
[![GitHub](https://img.shields.io/github/license/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter?color=594ae2&logo=github)](https://github.com/ScarletKuro/Scarlet.System.Text.Json.DateTimeConverter/blob/master/LICENSE)

This package allows you to specify a custom date format for `DateTime`, `DateTimeOffset`, and their nullable counterparts when serializing and deserializing JSON using `System.Text.Json`.

## Installation

To install the **Scarlet.System.Text.Json.DateTimeConverter** package, run the following command in your terminal:

```bash
dotnet add package Scarlet.System.Text.Json.DateTimeConverter
```

### Prerequisites

Make sure you have the appropriate .NET target framework installed. This package is compatible with the following versions:

- .NET 6
- .NET 7
- .NET 8

## Usage

Examples of how to serialize and deserialize models with custom date formats using `JsonDateTimeConverter` and `JsonDateTimeFormatConverter`. Note the differences when using source generators with `System.Text.Json`.

### Using reflection based with `JsonDateTimeConverter`

```csharp
public class MyModel
{
    [JsonDateTimeConverter("yyyy-MM-dd")]
    public DateTime Date { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset DateTimeOffset { get; set; }
}

public class Program
{
    public static void Main()
    {
        var model = new MyModel
        {
            Date = DateTime.Now,
            DateTimeOffset = DateTimeOffset.Now
        };

        // Serialize
        string jsonString = JsonSerializer.Serialize(model);
        Console.WriteLine($"Serialized JSON: {jsonString}");

        // Deserialize
        var deserializedModel = JsonSerializer.Deserialize<MyModel>(jsonString);
        Console.WriteLine($"Deserialized Date: {deserializedModel.Date}");
        Console.WriteLine($"Deserialized DateTimeOffset: {deserializedModel.DateTimeOffset}");
    }
}
```

**Note:** `JsonDateTimeConverter` does not support `System.Text.Json` source generators. Using this converter with `JsonSerializerContext` results in **SYSLIB1223**: "Attributes deriving from `JsonConverterAttribute` are not supported by the source generator." For such cases, use `JsonDateTimeFormatConverter`.

### Using Source Generators with `JsonDateTimeFormatConverter`

To work with `System.Text.Json` source generators, use `JsonDateTimeFormatConverter` instead of `JsonDateTimeConverterAttribute`.

#### Example Model

```csharp
public class MyModelSourceGenerator
{
    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeFormat>))]
    public DateTime Date { get; set; }

    [JsonConverter(typeof(JsonDateTimeFormatConverter<JsonDateTimeFormat.DateTimeOffsetFormat>))]
    public DateTimeOffset DateTimeOffset { get; set; }
}

internal class JsonDateTimeFormat
{
    internal class DateTimeOffsetFormat : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss.fffZ";
    }
    
    internal class DateTimeFormat : IJsonDateTimeFormat
    {
        public static string Format => "yyyy-MM-ddTHH:mm:ss";
    }
}

[JsonSerializable(typeof(MyModelSourceGenerator))]
public sealed partial class MyModelSourceGeneratorJsonSerializerContext : JsonSerializerContext;

public class Program
{
    public static void Main()
    {
        var modelType = typeof(MyModelSourceGenerator);
        var model = new MyModelSourceGenerator
        {
            Date = DateTime.Now,
            DateTimeOffset = DateTimeOffset.Now
        };

        var context = MyModelSourceGeneratorJsonSerializerContext.Default;

        // Serialize
        string jsonString = JsonSerializer.Serialize(model, modelType, context);
        Console.WriteLine($"Serialized JSON: {jsonString}");

        // Deserialize
        var deserializedModel = (MyModelSourceGenerator?)JsonSerializer.Deserialize(jsonString, modelType, context);
        Console.WriteLine($"Deserialized Date: {deserializedModel.Date}");
        Console.WriteLine($"Deserialized DateTimeOffset: {deserializedModel.DateTimeOffset}");
    }
}
```

Unfortunately, there is no better way with the source generator than defining a class for each date-time format. This is because the `JsonConverterAttribute` is not supported by the source generator, and neither `JsonConverterFactory` nor `JsonConverter` allows passing the format string to the converter, as they lack constructors with parameters.
The new contract customization does not provide attribute support for the source generator as well.

## Notes

- The `JsonDateTimeConverterAttribute` and `JsonDateTimeFormatConverter` can be applied to properties of type `DateTime`, `DateTime?`, `DateTimeOffset`, and `DateTimeOffset?`.
- The format string provided to the attribute should follow the standard date and time format strings in .NET.
