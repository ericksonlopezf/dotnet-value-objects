// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Universal <see cref="JsonConverterFactory"/> for automatically resolving and instantiating
/// System.Text.Json converters for all Value Objects in the EricksonLopez ecosystem.
/// </summary>
public sealed class ValueObjectJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Reflection on interfaces is safe for registered types.")]
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert, nameof(typeToConvert));

        if (typeToConvert == typeof(Money) || typeToConvert == typeof(CurrencyCode))
        {
            return true;
        }

        if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Range<>))
        {
            return true;
        }

        if (IsParsableType(typeToConvert))
        {
            return true;
        }

        if (FindGenericBase(typeToConvert, "SingleValueObject`2") != null)
        {
            return true;
        }

        return false;
    }

    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Reflection on interfaces is safe for registered types.")]
    private static bool IsParsableType(Type type)
    {
        if (type.IsPrimitive || type.Namespace?.StartsWith("System", StringComparison.Ordinal) == true || type == typeof(Money) || type == typeof(CurrencyCode))
        {
            return false;
        }

        foreach (var iface in type.GetInterfaces())
        {
            if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IParsable<>))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Explicitly registers a pre-instantiated <see cref="JsonConverter{T}"/> to guarantee Native AOT and Trimming compatibility without runtime reflection.
    /// </summary>
    /// <typeparam name="T">The type to convert.</typeparam>
    /// <param name="converter">The converter instance to register.</param>
    public static void RegisterConverter<T>(JsonConverter<T> converter)
    {
        ArgumentNullException.ThrowIfNull(converter, nameof(converter));
        Cache[typeof(T)] = converter;
    }

    internal static void ClearCache() => Cache.Clear();

    /// <inheritdoc/>
    [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Factory dynamically registers Value Object converters.")]
    [UnconditionalSuppressMessage("Trimming", "IL2055", Justification = "Factory dynamically registers Value Object converters.")]
    [UnconditionalSuppressMessage("Trimming", "IL2070", Justification = "Factory dynamically registers Value Object converters.")]
    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Factory dynamically registers Value Object converters.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert, nameof(typeToConvert));

        if (Cache.TryGetValue(typeToConvert, out var cached))
        {
            return cached;
        }

        if (typeToConvert == typeof(Money))
        {
            var moneyConverter = new MoneyJsonConverter();
            Cache.TryAdd(typeToConvert, moneyConverter);
            return moneyConverter;
        }

        if (typeToConvert == typeof(CurrencyCode))
        {
            var currencyConverter = new CurrencyCodeJsonConverter();
            Cache.TryAdd(typeToConvert, currencyConverter);
            return currencyConverter;
        }

        if (typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Range<>))
        {
            var valueType = typeToConvert.GetGenericArguments()[0];
            JsonConverter rangeConverter;
            if (valueType == typeof(int))
            {
                rangeConverter = new RangeJsonConverter<int>();
            }
            else if (valueType == typeof(decimal))
            {
                rangeConverter = new RangeJsonConverter<decimal>();
            }
            else if (valueType == typeof(double))
            {
                rangeConverter = new RangeJsonConverter<double>();
            }
            else if (valueType == typeof(DateTime))
            {
                rangeConverter = new RangeJsonConverter<DateTime>();
            }
            else if (valueType == typeof(DateOnly))
            {
                rangeConverter = new RangeJsonConverter<DateOnly>();
            }
            else
            {
                var converterType = typeof(RangeJsonConverter<>).MakeGenericType(valueType);
                rangeConverter = (JsonConverter)Activator.CreateInstance(converterType)!;
            }

            Cache.TryAdd(typeToConvert, rangeConverter);
            return rangeConverter;
        }

        // Generic parsable single-value object
        if (IsParsableType(typeToConvert))
        {
            var converterType = typeof(ParsableJsonConverter<>).MakeGenericType(typeToConvert);
            var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
            Cache.TryAdd(typeToConvert, converter);
            return converter;
        }

        // Check if it derives from SingleValueObject<TSelf, TValue>
        var svoBase = FindGenericBase(typeToConvert, "SingleValueObject`2");
        if (svoBase != null)
        {
            var tValue = svoBase.GetGenericArguments()[1];
            var converterType = typeof(ReflectionSingleValueObjectJsonConverter<,>).MakeGenericType(typeToConvert, tValue);
            var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
            Cache.TryAdd(typeToConvert, converter);
            return converter;
        }

        return null;
    }

    private static Type? FindGenericBase(Type type, string genericTypeName)
    {
        var current = type;
        while (current != null && current != typeof(object))
        {
            if (current.IsGenericType && string.Equals(current.GetGenericTypeDefinition().Name, genericTypeName, StringComparison.Ordinal))
            {
                return current;
            }
            current = current.BaseType;
        }
        return null;
    }

    private sealed class CurrencyCodeJsonConverter : JsonConverter<CurrencyCode>
    {
        public override CurrencyCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return default;
            }

            return CurrencyCode.Parse(str, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, CurrencyCode value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer, nameof(writer));
            if (!value.IsInitialized)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.Value);
        }
    }

    private sealed class ParsableJsonConverter<T> : JsonConverter<T> where T : IParsable<T>
    {
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return default;
            }

            var str = reader.GetString()!;
            return T.Parse(str, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer, nameof(writer));
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value.ToString());
        }
    }

    private sealed class ReflectionSingleValueObjectJsonConverter<TSelf, TValue> : JsonConverter<TSelf>
        where TSelf : class
        where TValue : notnull
    {
        private readonly MethodInfo? _createMethod;

        [UnconditionalSuppressMessage("Trimming", "IL2065", Justification = "Reflection lookup on targetType is safe.")]
        [UnconditionalSuppressMessage("Trimming", "IL2090", Justification = "Reflection lookup on TSelf is safe.")]
        public ReflectionSingleValueObjectJsonConverter()
        {
            Type targetType = typeof(TSelf);
            if (targetType.IsAbstract && targetType.IsGenericType && string.Equals(targetType.GetGenericTypeDefinition().Name, "SingleValueObject`2", StringComparison.Ordinal))
            {
                targetType = targetType.GetGenericArguments()[0];
            }
            _createMethod = targetType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static, [typeof(TValue)]);
        }

        public override bool HandleNull => true;

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Converter resolution for TValue is safe.")]
        [UnconditionalSuppressMessage("Trimming", "IL2075", Justification = "Reflection on Result type is safe.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Converter resolution for TValue is safe.")]
        public override TSelf? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
            var primitiveValue = valueConverter.Read(ref reader, typeof(TValue), options);
            if (primitiveValue is null)
            {
                return null;
            }

            if (_createMethod != null)
            {
                var result = _createMethod.Invoke(null, [primitiveValue]);
                if (result != null)
                {
                    var isSuccessProp = result.GetType().GetProperty("IsSuccess");
                    if (isSuccessProp != null && (bool)isSuccessProp.GetValue(result)!)
                    {
                        var valueProp = result.GetType().GetProperty("Value");
                        return (TSelf?)valueProp?.GetValue(result);
                    }
                }
            }

            throw new JsonException($"Cannot instantiate '{typeof(TSelf).Name}' from '{primitiveValue}'.");
        }

        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Converter resolution for TValue is safe.")]
        [UnconditionalSuppressMessage("Trimming", "IL2090", Justification = "Property lookup on TSelf is safe.")]
        [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "Converter resolution for TValue is safe.")]
        public override void Write(Utf8JsonWriter writer, TSelf value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer, nameof(writer));
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            var prop = typeof(TSelf).GetProperty("Value");
            var rawValue = (TValue?)prop?.GetValue(value);

            var valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
            valueConverter.Write(writer, rawValue!, options);
        }
    }
}
