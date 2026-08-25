// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Provides a <see cref="JsonConverter{T}"/> for serializing and deserializing <see cref="Range{T}"/> instances.
/// </summary>
/// <typeparam name="T">The underlying value type contained in the range.</typeparam>
public sealed class RangeJsonConverter<T> : JsonConverter<Range<T>>
    where T : struct, IComparable<T>, IEquatable<T>
{
    /// <inheritdoc/>
    [UnconditionalSuppressMessage("AOT", "IL2026:RequiresUnreferencedCode", Justification = "Converter lookup is resolved for known primitives or registered JsonSerializerContexts.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "AOT compilation preserves types referenced in JsonTypeInfo.")]
    public override Range<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject token when deserializing Range.");
        }

        var converter = (JsonConverter<T>)options.GetConverter(typeof(T));
        T? start = null;
        T? end = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (string.Equals(propertyName, nameof(Range<T>.Start), StringComparison.OrdinalIgnoreCase))
                {
                    start = converter.Read(ref reader, typeof(T), options);
                }
                else if (string.Equals(propertyName, nameof(Range<T>.End), StringComparison.OrdinalIgnoreCase))
                {
                    end = converter.Read(ref reader, typeof(T), options);
                }
            }
        }

        if (start is null || end is null)
        {
            throw new JsonException("Range requires both 'Start' and 'End' properties.");
        }

        var result = Range<T>.Create(start.Value, end.Value);
        if (result.IsFailure)
        {
            throw new JsonException($"Invalid Range deserialization: {result.Error.Description}");
        }

        return result.Value;
    }

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("AOT", "IL2026:RequiresUnreferencedCode", Justification = "Converter lookup is resolved for known primitives or registered JsonSerializerContexts.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "AOT compilation preserves types referenced in JsonTypeInfo.")]
    public override void Write(Utf8JsonWriter writer, Range<T> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer, nameof(writer));

        var converter = (JsonConverter<T>)options.GetConverter(typeof(T));

        writer.WriteStartObject();
        writer.WritePropertyName(nameof(Range<T>.Start));
        converter.Write(writer, value.Start, options);
        writer.WritePropertyName(nameof(Range<T>.End));
        converter.Write(writer, value.End, options);
        writer.WriteEndObject();
    }
}

