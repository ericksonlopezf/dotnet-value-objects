// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Provides a <see cref="JsonConverter{T}"/> for serializing and deserializing <see cref="Money"/> instances
/// in structured object format (<c>{"amount": 100.50, "currency": "USD"}</c>) or string format (<c>"100.50 USD"</c>).
/// </summary>
public sealed class MoneyJsonConverter : JsonConverter<Money>
{
    /// <inheritdoc/>
    public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return default;
            }
            return Money.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        }

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Expected StartObject or String token when deserializing Money, got '{reader.TokenType}'.");
        }

        decimal? amount = null;
        string? currencyStr = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                if (reader.ValueTextEquals("amount"u8))
                {
                    reader.Read();
                    amount = reader.GetDecimal();
                }
                else if (reader.ValueTextEquals("currency"u8))
                {
                    reader.Read();
                    currencyStr = reader.GetString();
                }
                else
                {
                    reader.Skip();
                }
            }
        }

        if (amount is null)
        {
            throw new JsonException("Money object requires an 'amount' property.");
        }

        if (string.IsNullOrWhiteSpace(currencyStr))
        {
            throw new JsonException("Money object requires a 'currency' property.");
        }

        var currResult = CurrencyCode.Create(currencyStr);
        if (currResult.IsFailure)
        {
            throw new JsonException($"Invalid currency code '{currencyStr}': {currResult.Error.Description}");
        }

        var moneyResult = Money.Create(amount.Value, currResult.Value);
        if (moneyResult.IsFailure)
        {
            throw new JsonException($"Failed to deserialize Money: {moneyResult.Error.Description}");
        }

        return moneyResult.Value;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer, nameof(writer));

        writer.WriteStartObject();
        writer.WriteNumber("amount"u8, value.Amount);
        writer.WriteString("currency"u8, value.Currency.Value ?? string.Empty);
        writer.WriteEndObject();
    }
}
