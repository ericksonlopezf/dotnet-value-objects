// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Provides a generic <see cref="JsonConverter{T}"/> for single-value Value Objects.
/// </summary>
/// <typeparam name="TSelf">The concrete Value Object type.</typeparam>
/// <typeparam name="TValue">The underlying primitive value type.</typeparam>
public abstract class SingleValueObjectJsonConverter<TSelf, TValue> : JsonConverter<TSelf>
    where TSelf : SingleValueObject<TSelf, TValue>
    where TValue : notnull
{
    /// <inheritdoc/>
    [UnconditionalSuppressMessage("AOT", "IL2026:RequiresUnreferencedCode", Justification = "Converter lookup is resolved for known primitives or registered JsonSerializerContexts.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "AOT compilation preserves types referenced in JsonTypeInfo.")]
    public override TSelf? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var converter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
        var primitiveValue = converter.Read(ref reader, typeof(TValue), options);

        if (primitiveValue is null)
        {
            return null;
        }

        var result = CreateInstance(primitiveValue);
        if (result.IsFailure)
        {
            throw new JsonException($"Failed to deserialize '{typeof(TSelf).Name}': {result.Error.Description}");
        }

        return result.Value;
    }

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("AOT", "IL2026:RequiresUnreferencedCode", Justification = "Converter lookup is resolved for known primitives or registered JsonSerializerContexts.")]
    [UnconditionalSuppressMessage("AOT", "IL3050:RequiresDynamicCode", Justification = "AOT compilation preserves types referenced in JsonTypeInfo.")]
    public override void Write(Utf8JsonWriter writer, TSelf value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer, nameof(writer));
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var converter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
        converter.Write(writer, value.Value, options);
    }

    /// <summary>
    /// Creates a new instance of the Value Object from its primitive representation.
    /// </summary>
    /// <param name="value">The underlying primitive value to wrap.</param>
    /// <returns>A <see cref="Result{TSelf}"/> containing the created Value Object or a validation error.</returns>
    protected abstract Result<TSelf> CreateInstance(TValue value);
}

