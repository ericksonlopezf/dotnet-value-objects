// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="TaxRate"/>.
/// </summary>
public sealed class TaxRateValueConverter : ValueConverter<TaxRate, decimal>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaxRateValueConverter"/> class.
    /// </summary>
    public TaxRateValueConverter()
        : base(
            tax => tax.Value,
            value => TaxRate.Create(value).Value)
    {
    }
}

