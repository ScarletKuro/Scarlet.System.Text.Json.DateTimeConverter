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
│       └── release.yml
├── src/
│   ├── Scarlet.System.Text.Json.DateTimeConverter/
│   └── Scarlet.System.Text.Json.DateTimeConverter.Tests/
├── README.md
├── LICENSE
├── version.json
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
dotnet test tests/Scarlet.System.Text.Json.DateTimeConverter.Tests/Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj -c Release --no-build
dotnet pack src/Scarlet.System.Text.Json.DateTimeConverter/Scarlet.System.Text.Json.DateTimeConverter.csproj -c Release --no-build -o artifacts/packages
```

### Coverage and Test Result Artifacts

Use the same outputs as CI when validating locally:

```bash
dotnet test tests/Scarlet.System.Text.Json.DateTimeConverter.Tests/Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj -c Release --no-build -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura -p:CoverletOutput=../../artifacts/coverage/Scarlet.System.Text.Json.DateTimeConverter.Tests.xml --logger "junit;LogFilePath=../../artifacts/test-results/Scarlet.System.Text.Json.DateTimeConverter.Tests.xml"
```

Expected artifact folders:

- `artifacts/test-results`
- `artifacts/coverage`
- `artifacts/packages`

## Test Guidance

### Run All Tests

```bash
dotnet test tests/Scarlet.System.Text.Json.DateTimeConverter.Tests/Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj -c Release
```

### Run a Specific Test

```bash
dotnet test tests/Scarlet.System.Text.Json.DateTimeConverter.Tests/Scarlet.System.Text.Json.DateTimeConverter.Tests.csproj -c Release --filter "FullyQualifiedName~ReflectionBased_DateTime_WithAttribute"
```

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

Versioning is handled by [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning).

To publish a release:

1. Update `version.json` if needed.
2. Create a version tag such as `1.2.0`.
3. Push the tag to GitHub.
4. GitHub Actions runs `.github/workflows/release.yml`.
5. The workflow builds, packs, publishes `.nupkg` files to NuGet.org and GitHub Packages, and uploads the package artifacts.

### Required Secrets

- `NUGET_KEY` for NuGet.org publishing
- `GITHUB_TOKEN` for GitHub Packages publishing
- `CODECOV_TOKEN` for CI coverage and test-result uploads

## CI/CD

### Continuous Integration

`.github/workflows/continuous.yml` runs on every push and performs:

- Restore
- Formatting verification
- Release build
- Test execution with JUnit results
- Cobertura coverage generation
- NuGet package creation
- Codecov uploads

### Release Workflow

`.github/workflows/release.yml` runs on version tag pushes matching `*.*.*` and performs:

- Restore
- Release build
- NuGet package creation
- Publish to NuGet.org
- Publish to GitHub Packages

## Troubleshooting

### Versioning Fails in CI

If version height calculation fails locally because of shallow history:

```bash
git fetch --unshallow
```

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
- [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning)
