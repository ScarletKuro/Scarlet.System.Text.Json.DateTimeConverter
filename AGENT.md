# AGENT.md

This document provides guidance for AI agents and developers working on the **Scarlet.System.Text.Json.DateTimeConverter** package.

## Package Overview

**Scarlet.System.Text.Json.DateTimeConverter** is a .NET library that provides flexible custom date/time formatting for `System.Text.Json` serialization. It supports both reflection-based and source generator approaches, with special .NET 9+ features for enhanced source generator compatibility.

### Key Features

- Custom date/time format attributes (`JsonDateTimeConverterAttribute` for reflection, `JsonDateTimeFormatAttribute` for source generators)
- Source generator-compatible format converters (`JsonDateTimeFormatConverter<T>`)
- .NET 9+ contract customization resolver (`DateTimeConverterResolver`)
- Support for `DateTime`, `DateTimeOffset`, and nullable variants
- Multi-target framework support (.NET 6, 7, 8, 9, 10)

## Repository Structure

```
Scarlet.System.Text.Json.DateTimeConverter/
├── README.md                     # User-facing documentation
├── LICENSE                       # MIT License
├── version.json                  # Nerdbank.GitVersioning configuration
├── icon.png                      # Package icon
├── build/                        # NUKE build system
│   ├── Build.cs                  # Main build configuration
│   └── _build.csproj             # Build project
├── src/
│   ├── Scarlet.System.Text.Json.DateTimeConverter/
│   │   ├── Converters/           # Internal converter implementations
│   │   │   ├── DateTimeConverter.cs
│   │   │   ├── DateTimeNullableConverter.cs
│   │   │   ├── DateTimeOffsetConverter.cs
│   │   │   └── DateTimeOffsetNullableConverter.cs
│   │   ├── DateTimeConverterFactoryHelper.cs
│   │   ├── DateTimeConverterResolver.cs  # .NET 9+ contract customization
│   │   ├── IJsonDateTimeFormat.cs
│   │   ├── JsonDateTimeConverterAttribute.cs  # For reflection-based serialization
│   │   ├── JsonDateTimeFormatAttribute.cs     # For source generators (no warnings)
│   │   ├── JsonDateTimeFormatConverter.cs
│   │   └── Scarlet.System.Text.Json.DateTimeConverter.csproj
│   └── Scarlet.System.Text.Json.DateTimeConverter.Tests/
│       ├── JsonDateTimeConverterAttributeTests.cs
│       ├── JsonDateTimeFormatConverterTests.cs
│       ├── Model/                # Test models
│       │   ├── ReflectionBasedModel.cs
│       │   ├── SourceGeneratorWithConverterModel.cs
│       │   ├── SourceGeneratorWithResolverAttributeModel.cs  # Uses JsonDateTimeConverter (has warnings)
│       │   ├── SourceGeneratorWithResolverFormatModel.cs     # Uses JsonDateTimeFormat (no warnings)
│       │   ├── ConverterModelJsonSerializerContext.cs
│       │   └── ResolverModelJsonSerializerContext.cs
│       └── Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj
└── .github/
    └── workflows/                # CI/CD workflows
        ├── continuous.yml        # Build and test on push
        └── release.yml           # Publish on tag
```

## Building the Project

### Prerequisites

- .NET SDK 10.0 or later (for development)
- Git (for version control)

### Build Commands

The project uses [NUKE](https://nuke.build/) for build automation.

#### Quick Build

```bash
# Linux/macOS
./build.sh Compile

# Windows
build.cmd Compile
```

#### Clean Build

```bash
# Clean + Restore + Build
./build.sh Clean Compile

# Or use standard dotnet commands
dotnet clean
dotnet build src/Scarlet.System.Text.Json.DateTimeConverter.sln --configuration Release
```

#### Full CI Build

```bash
./build.sh Clean Restore VerifyFormat Compile Test Pack
```

### Build Targets

| Target | Description |
|--------|-------------|
| `Clean` | Cleans build artifacts |
| `Restore` | Restores NuGet packages |
| `VerifyFormat` | Verifies code formatting (whitespace and style) |
| `Compile` | Compiles the solution |
| `Test` | Runs all tests |
| `Pack` | Creates NuGet packages |
| `Push` | Pushes packages to NuGet.org (requires tag + secrets) |
| `PushGithubNuget` | Pushes packages to GitHub Packages (requires tag + secrets) |

## Running Tests

### Run All Tests

```bash
# Using NUKE
./build.sh Test

# Using dotnet CLI
dotnet test src/Scarlet.System.Text.Json.DateTimeConverter.Tests/Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj --configuration Release
```

### Run Specific Test

```bash
dotnet test --filter "FullyQualifiedName~ReflectionBased_DateTime_WithAttribute"
```

### Test Project Structure

Tests are organized into two main test classes:

1. **`JsonDateTimeConverterAttributeTests.cs`**
   - Tests for `JsonDateTimeConverterAttribute` with reflection-based serialization
   - Tests individual primitive types and complete models
   
2. **`JsonDateTimeFormatConverterTests.cs`**
   - Tests for `JsonDateTimeFormatConverter<T>` with both reflection and source generators
   - Tests for `DateTimeConverterResolver` (.NET 9+)
   - Includes tests with null values

### Test Naming Convention

Tests follow the pattern: `[ApproachType]_[Scenario]_[OptionalDetails]`

Examples:
- `ReflectionBased_DateTime_WithAttribute`
- `SourceGenerator_CompleteModel_WithFormatConverter`
- `SourceGenerator_WithResolver_WithAttribute_UsingOptions`

## Development Workflow

### 1. Make Changes

Edit source files in `src/Scarlet.System.Text.Json.DateTimeConverter/`

### 2. Format Code

The project enforces code formatting. Use:

```bash
# Check format
dotnet format whitespace src/Scarlet.System.Text.Json.DateTimeConverter.sln --verify-no-changes
dotnet format style src/Scarlet.System.Text.Json.DateTimeConverter.sln --verify-no-changes

# Fix format
dotnet format whitespace src/Scarlet.System.Text.Json.DateTimeConverter.sln
dotnet format style src/Scarlet.System.Text.Json.DateTimeConverter.sln
```

### 3. Build and Test

```bash
./build.sh Compile Test
```

### 4. Create PR

The project uses a standard GitHub workflow:
1. Fork or create a branch
2. Make changes
3. Ensure tests pass
4. Create pull request

## Architecture

### Core Components

#### 1. Converters (Internal)

Four internal converter classes handle actual JSON serialization/deserialization:
- `DateTimeConverter` - for `DateTime`
- `DateTimeNullableConverter` - for `DateTime?`
- `DateTimeOffsetConverter` - for `DateTimeOffset`
- `DateTimeOffsetNullableConverter` - for `DateTimeOffset?`

All use `CultureInfo.InvariantCulture` for consistent formatting.

#### 2. Public API

**`JsonDateTimeConverterAttribute`**
```csharp
[JsonDateTimeConverter("yyyy-MM-dd")]
public DateTime Date { get; set; }
```
- Derives from `JsonConverterAttribute`
- Works with reflection-based serialization
- .NET 9+: Works with source generators via `DateTimeConverterResolver` but produces SYSLIB1223 warnings

**`JsonDateTimeFormatAttribute` (.NET 9+)**
```csharp
[JsonDateTimeFormat("yyyy-MM-dd")]
public DateTime Date { get; set; }
```
- Derives from `Attribute` (not `JsonConverterAttribute`)
- Designed for use with .NET 9+ source generators and `DateTimeConverterResolver`
- **No SYSLIB1223 warnings** (recommended over `JsonDateTimeConverterAttribute` for source generators)

**`JsonDateTimeFormatConverter<T>`**
```csharp
[JsonConverter(typeof(JsonDateTimeFormatConverter<MyFormat>))]
public DateTime Date { get; set; }
```
- `JsonConverterFactory` implementation
- Compatible with source generators (all .NET versions)
- Requires `IJsonDateTimeFormat` implementation

**`DateTimeConverterResolver` (.NET 9+)**
```csharp
var options = new JsonSerializerOptions
{
    TypeInfoResolver = new DateTimeConverterResolver(MyJsonContext.Default)
};
```
- Implements `IJsonTypeInfoResolver` and extends `JsonSerializerContext`
- Uses `JsonPropertyInfo.AttributeProvider` (populated by source generators in .NET 9+) to read attributes
- Supports both `JsonDateTimeFormatAttribute` (no warnings) and `JsonDateTimeConverterAttribute` (backward compatibility)
- Enables attribute syntax with source generators

#### 3. Factory Helper

`DateTimeConverterFactoryHelper` centralizes converter instantiation based on target type.

### Multi-Targeting

The library targets multiple frameworks to maximize compatibility:

```xml
<TargetFrameworks>net6.0;net7.0;net8.0;net9.0;net10.0</TargetFrameworks>
```

`DateTimeConverterResolver` is conditionally compiled for .NET 9+ only:

```csharp
#if NET9_0_OR_GREATER
public class DateTimeConverterResolver : JsonSerializerContext, IJsonTypeInfoResolver
{
    // Implementation
}
#endif
```

### Testing Strategy

Tests verify four distinct usage patterns:

1. **Reflection-based** - Uses `JsonDateTimeConverterAttribute` with default `JsonSerializer`
2. **Source generator with converter** - Uses `JsonDateTimeFormatConverter<T>` with `JsonSerializerContext`
3. **Source generator with resolver (new attribute)** - Uses `JsonDateTimeFormatAttribute` + `DateTimeConverterResolver` (.NET 9+, no warnings)
4. **Source generator with resolver (old attribute)** - Uses `JsonDateTimeConverterAttribute` + `DateTimeConverterResolver` (.NET 9+, with SYSLIB1223 warnings for backward compatibility)

Each pattern is tested with:
- Individual types (`DateTime`, `DateTime?`, `DateTimeOffset`, `DateTimeOffset?`)
- Complete models
- Null value handling

## Common Tasks

### Adding a New Converter

1. Create converter in `Converters/` directory
2. Implement `JsonConverter<T>` with `Read` and `Write` methods
3. Register in `DateTimeConverterFactoryHelper.CreateConverter`
4. Add tests in both test files
5. Update documentation

### Updating Supported Frameworks

1. Update `<TargetFrameworks>` in `.csproj`
2. Test all scenarios on new framework
3. Update `README.md` prerequisites
4. Update CI/CD workflows if needed

### Releasing a New Version

Versioning is handled by [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning):

1. Update `version.json` if needed
2. Create a git tag: `git tag 1.2.0`
3. Push tag: `git push origin 1.2.0`
4. GitHub Actions will automatically build and publish

## Troubleshooting

### Build Issues

**Problem:** "Shallow clone lacks the objects required to calculate version height"

**Solution:** 
```bash
git fetch --unshallow
```

**Problem:** Format verification fails

**Solution:**
```bash
dotnet format whitespace src/Scarlet.System.Text.Json.DateTimeConverter.sln
dotnet format style src/Scarlet.System.Text.Json.DateTimeConverter.sln
```

### Test Issues

**Problem:** SYSLIB1223 warning in tests

**Solution:** This is expected for `SourceGeneratorWithResolverModel` as it demonstrates the problem that `DateTimeConverterResolver` solves.

**Problem:** Tests fail after renaming models

**Solution:** Ensure all references are updated in both test files and `TestModelSourceGeneratorJsonSerializerContext.cs`.

## CI/CD

### Continuous Integration

On every push:
- Restore packages
- Verify code format
- Compile all target frameworks
- Run tests
- Create NuGet packages (artifacts)

### Release

On tag push (e.g., `1.2.0`):
- All CI steps
- Publish to NuGet.org
- Publish to GitHub Packages

### Secrets Required

- `NUGET_KEY` - NuGet.org API key
- `GITHUB_TOKEN` - Automatically provided by GitHub Actions

## Code Style

- Use C# 12+ features where appropriate
- Nullable reference types enabled
- `ImplicitUsings` enabled
- Follow .editorconfig rules
- Use XML documentation for public APIs
- Keep internal converters simple and focused

## Performance Considerations

- Converters use `CultureInfo.InvariantCulture` for consistent, culture-independent formatting
- No reflection in hot path (converters are created once and reused)
- Source generator support ensures zero reflection overhead when using AOT

## Additional Resources

- [System.Text.Json Documentation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [Custom Converters](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to)
- [Source Generation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
- [Contract Customization (.NET 9+)](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts)
- [NUKE Build](https://nuke.build/)
- [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning)
