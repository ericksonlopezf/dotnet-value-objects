// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Provides a specialized <see cref="JsonConverter{T}"/> for string-backed Value Objects.
/// </summary>
/// <typeparam name="TSelf">The concrete string Value Object type.</typeparam>
public abstract class StringValueObjectJsonConverter<TSelf> : JsonConverter<TSelf>
    where TSelf : SingleValueObject<TSelf, string>
{
    /// <inheritdoc/>
    public override TSelf? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        var str = reader.GetString()!;
        var result = CreateInstance(str);
        if (result.IsFailure)
        {
            throw new JsonException($"Failed to deserialize '{typeof(TSelf).Name}': {result.Error.Description}");
        }

        return result.Value;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TSelf value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer, nameof(writer));
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStringValue(value.Value);
    }

    /// <summary>
    /// Creates a new instance of the string Value Object from its string representation.
    /// </summary>
    /// <param name="value">The underlying string value to wrap.</param>
    /// <returns>A <see cref="Result{TSelf}"/> containing the created Value Object or a validation error.</returns>
    protected abstract Result<TSelf> CreateInstance(string value);
}

