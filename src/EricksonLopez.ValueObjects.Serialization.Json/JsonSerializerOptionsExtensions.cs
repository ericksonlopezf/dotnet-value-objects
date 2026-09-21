// Copyright © Erickson Lopez. MIT License.
using System.Text.Json;

namespace EricksonLopez.ValueObjects.Serialization.Json;

/// <summary>
/// Provides extension methods for <see cref="JsonSerializerOptions"/> to safely register Value Object serializers.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    /// Registers the <see cref="ValueObjectJsonConverterFactory"/> in the specified <see cref="JsonSerializerOptions"/> 
    /// to ensure that all Value Objects are correctly validated and instantiated via their domain factories during deserialization.
    /// </summary>
    /// <param name="options">The JSON serializer options to configure.</param>
    /// <returns>The modified options instance to allow chaining.</returns>
    public static JsonSerializerOptions AddValueObjectConverters(this JsonSerializerOptions options)
    {
        System.ArgumentNullException.ThrowIfNull(options);

        if (!System.Linq.Enumerable.Any(options.Converters, c => c is ValueObjectJsonConverterFactory))
        {
            options.Converters.Add(new ValueObjectJsonConverterFactory());
        }

        return options;
    }
}
