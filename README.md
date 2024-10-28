## Overview

The `JsonDateTimeConverterAttribute` allows you to specify a custom date format for `DateTime`, `DateTimeOffset`, and their nullable counterparts when serializing and deserializing JSON using `System.Text.Json`. This ensures consistency in how date and time values are handled across your application.

## Installation

Ensure you have the necessary .NET target framework installed. This attribute is compatible with:
- .NET 6
- .NET 7
- .NET 8

## Usage

### Example Model

```csharp
using System;
using System.Text.Json.Serialization;
using Scarlet.System.Text.Json.DateTimeConverter;

public class MyModel
{
    [JsonDateTimeConverter("yyyy-MM-dd")]
    public DateTime Date { get; set; }

    [JsonDateTimeConverter("yyyy-MM-ddTHH:mm:ss.fffZ")]
    public DateTimeOffset DateTimeOffset { get; set; }
}
```

### Example Program

```csharp
using System;
using System.Text.Json;

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

## Notes

- The `JsonDateTimeConverterAttribute` can be applied to properties of type `DateTime`, `DateTime?`, `DateTimeOffset`, and `DateTimeOffset?`.
- The format string provided to the attribute should follow the standard date and time format strings in .NET.
