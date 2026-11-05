# AGENT.md

This document provides guidance for AI agents and developers working on the **Scarlet.System.Text.Json.DateTimeConverter** package.

## Package Overview

**Scarlet.System.Text.Json.DateTimeConverter** is a .NET library that provides flexible custom date/time formatting for `System.Text.Json` serialization. It supports both reflection-based and source generator approaches, with .NET 9+ features for source generator-friendly attribute handling.

### Key Features

- Custom date/time format attributes for reflection and source generator usage
- Source generator-compatible format converters
- .NET 9+ contract customization resolver
- Support for `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, and nullable variants
- Multi-target framework support for .NET 6 through .NET 10

## Repository Structure

```text
Scarlet.System.Text.Json.DateTimeConverter/
├── .github/
│   └── workflows/
│       ├── continuous.yml
│       ├── release.yml
│       └── test.yml
├── src/
│   └── Scarlet.System.Text.Json.DateTimeConverter/
├── tests/
│   └── Scarlet.System.Text.Json.DateTimeConverter.Tests/
├── global.json
├── README.md
├── LICENSE
└── icon.png
```

## Local Development

### Prerequisites

- .NET SDK 10.0 or later
- Git

### Restore, Build, Test, Pack

```bash
dotnet restore Scarlet.System.Text.Json.DateTimeConverter.slnx
dotnet format whitespace Scarlet.System.Text.Json.DateTimeConverter.slnx --verify-no-changes
dotnet format style Scarlet.System.Text.Json.DateTimeConverter.slnx --verify-no-changes
dotnet build Scarlet.System.Text.Json.DateTimeConverter.slnx -c Release --no-restore
dotnet test --project tests/Scarlet.System.Text.Json.DateTimeConverter.Tests -c Release --no-build
dotnet pack src/Scarlet.System.Text.Json.DateTimeConverter/Scarlet.System.Text.Json.DateTimeConverter.csproj -c Release --no-build -o artifacts/packages
```

### Coverage and Test Result Artifacts

Use the same outputs as CI when validating locally:

```bash
dotnet test --project tests/Scarlet.System.Text.Json.DateTimeConverter.Tests -c Release --no-build --coverage --coverage-output-format cobertura --results-directory ./TestResults
```

Coverage is collected by Microsoft Code Coverage, which runs as a Microsoft.Testing.Platform extension. Reports are written to `TestResults/<guid>.cobertura.xml`; CI copies them to `artifacts/coverage`.

Expected artifact folders:

- `TestResults`
- `artifacts/coverage`
- `artifacts/packages`

## Test Guidance

Tests use xUnit v3 (`xunit.v3.aot`) on Microsoft.Testing.Platform, selected by `global.json`. The test project is an executable.

### Run All Tests

```bash
dotnet test --project tests/Scarlet.System.Text.Json.DateTimeConverter.Tests -c Release
```

### Run a Specific Test

```bash
dotnet test --project tests/Scarlet.System.Text.Json.DateTimeConverter.Tests -c Release --filter-method "*ReflectionBased_DateTime_WithAttribute*"
```

### Run Tests Under NativeAOT

The same tests, minus the reflection-based ones (`ReflectionBasedTests*.cs`), compiled with NativeAOT, serializer reflection disabled (`JsonSerializerIsReflectionEnabledByDefault=false`) and trim/AOT warnings treated as errors:

```bash
dotnet publish tests/Scarlet.System.Text.Json.DateTimeConverter.Tests -c Release -r linux-x64 -p:NativeAotTests=true -o artifacts/aot
./artifacts/aot/Scarlet.System.Text.Json.DateTimeConverter.Tests
```

On Windows use `-r win-x64` (requires the Visual Studio C++ build tools). Test code serializes through `TestJson`, which resolves `JsonTypeInfo<T>` from the options: reflection-based under JIT when no resolver is set, `PrimitiveJsonSerializerContext` under NativeAOT.

### Test Coverage Focus

Tests cover:

- Reflection-based serialization with `JsonDateTimeConverterAttribute`
- Source generator usage with `JsonDateTimeFormatConverter<T>`
- .NET 9+ resolver integration with `JsonDateTimeFormatAttribute`
- Backward compatibility for `JsonDateTimeConverterAttribute` with `DateTimeConverterResolver`
- Null handling for nullable converter paths

## Architecture

### Core Components

- `JsonDateTimeConverterAttribute`
  - Reflection-oriented attribute that derives from `JsonConverterAttribute`
- `JsonDateTimeFormatAttribute`
  - .NET 9+ attribute intended for source generator scenarios without `SYSLIB1223`
- `JsonDateTimeFormatConverter<T>`
  - Public converter factory for source generator-friendly formatting
- `DateTimeConverterResolver`
  - .NET 9+ `IJsonTypeInfoResolver`-based bridge for attribute-driven formatting
- `DateTimeConverterFactoryHelper`
  - Internal helper that instantiates the correct converter for supported primitive types

### Multi-Targeting

The main package targets:

```xml
<TargetFrameworks>net6.0;net7.0;net8.0;net9.0;net10.0</TargetFrameworks>
```

`DateTimeConverterResolver` is conditionally compiled for .NET 9 and later.

## Release Process

The release tag is the package version. Builds without `-p:Version` get the SDK default (`1.0.0`), and branch CI packages are versioned `0.0.0-ci.<run number>`.

To publish a release:

1. Create a SemVer tag such as `1.2.0` or `1.3.0-preview.1` (no `v` prefix).
2. Push the tag to GitHub.
3. GitHub Actions runs `.github/workflows/release.yml`.
4. The workflow validates the tag, then runs the full test workflow (NativeAOT included) on the tagged commit; only if it passes does it build, pack, publish `.nupkg` files to NuGet.org and GitHub Packages, and upload the package artifacts.

### Required Secrets

- `NUGET_KEY` for NuGet.org publishing
- `GITHUB_TOKEN` for GitHub Packages publishing
- `CODECOV_TOKEN` for CI coverage uploads

## CI/CD

### Test Workflow

`.github/workflows/test.yml` is a reusable workflow (`workflow_call`), called by both `continuous.yml` and `release.yml`. It performs:

- Restore
- Formatting verification
- Release build
- Test execution with Cobertura coverage (Microsoft Code Coverage)
- NativeAOT test publish and run
- Codecov upload (not fatal)

### Continuous Integration

`.github/workflows/continuous.yml` runs on every branch push (tags are handled by `release.yml`) and performs:

- The test workflow
- In parallel: restore, Release build, NuGet package creation and package artifact upload

### Release Workflow

`.github/workflows/release.yml` runs on version tag pushes matching `*.*.*` and performs:

- Tag format validation
- The test workflow, which must pass before anything is published
- Restore, Release build and NuGet package creation
- Publish to NuGet.org
- Publish to GitHub Packages
- Package artifact upload

## Troubleshooting

### Formatting Fails

```bash
dotnet format whitespace Scarlet.System.Text.Json.DateTimeConverter.slnx
dotnet format style Scarlet.System.Text.Json.DateTimeConverter.slnx
```

### Source Generator Warning in Compatibility Tests

`SYSLIB1223` is expected in tests that intentionally demonstrate backward compatibility with `JsonDateTimeConverterAttribute`.

## Additional Resources

- [System.Text.Json Documentation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [Custom Converters](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to)
- [Source Generation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
- [Contract Customization (.NET 9+)](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-contracts)