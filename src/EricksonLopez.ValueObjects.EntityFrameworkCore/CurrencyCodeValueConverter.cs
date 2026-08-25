// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="CurrencyCode"/>.
/// </summary>
public sealed class CurrencyCodeValueConverter : ValueConverter<CurrencyCode, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyCodeValueConverter"/> class.
    /// </summary>
    public CurrencyCodeValueConverter()
        : base(
            currency => currency.Value,
            value => CurrencyCode.Create(value).Value)
    {
    }
}

