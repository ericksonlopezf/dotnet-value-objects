// Copyright © Erickson Lopez. MIT License.
using System;

namespace Microsoft.EntityFrameworkCore;

using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides extension methods for configuring Value Object converters on <see cref="ModelConfigurationBuilder"/>.
/// </summary>
public static class ValueObjectModelConfigurationExtensions
{
    /// <summary>
    /// Registers default EF Core value converter mappings for core domain value objects.
    /// </summary>
    /// <param name="configurationBuilder">The model configuration builder to configure.</param>
    /// <returns>The configured <see cref="ModelConfigurationBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="configurationBuilder"/> is <see langword="null"/></exception>
    public static ModelConfigurationBuilder ConfigureDomainValueObjects(this ModelConfigurationBuilder configurationBuilder)
    {
        ArgumentNullException.ThrowIfNull(configurationBuilder);

        configurationBuilder.Properties<Email>().HaveConversion<EmailValueConverter>();
        configurationBuilder.Properties<PhoneNumber>().HaveConversion<PhoneNumberValueConverter>();
        configurationBuilder.Properties<PostalCode>().HaveConversion<PostalCodeValueConverter>();
        configurationBuilder.Properties<CurrencyCode>().HaveConversion<CurrencyCodeValueConverter>();
        configurationBuilder.Properties<Percentage>().HaveConversion<PercentageValueConverter>();
        configurationBuilder.Properties<TaxRate>().HaveConversion<TaxRateValueConverter>();
        configurationBuilder.Properties<Quantity>().HaveConversion<QuantityValueConverter>();

        return configurationBuilder;
    }
}


