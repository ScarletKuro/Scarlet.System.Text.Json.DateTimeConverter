namespace Scarlet.System.Text.Json.DateTimeConverter;

/// <summary>
/// Defines a contract for specifying a custom date format for JSON serialization and deserialization.
/// </summary>
/// <remarks>
/// This interface is used when you want to use source generator `System.Text.Json`, but it also works with non-source generator (reflection-based) serialization and deserialization.
/// </remarks>
public interface IJsonDateTimeFormat
{
#pragma warning disable CA2252
    /// <summary>
    /// Gets the date format string to be used for JSON serialization and deserialization.
    /// </summary>
    static abstract string Format { get; }
#pragma warning restore CA2252
}
