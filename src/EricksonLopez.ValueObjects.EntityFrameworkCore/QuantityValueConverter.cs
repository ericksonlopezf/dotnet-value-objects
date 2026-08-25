// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="Quantity"/>.
/// </summary>
public sealed class QuantityValueConverter : ValueConverter<Quantity, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuantityValueConverter"/> class.
    /// </summary>
    public QuantityValueConverter()
        : base(
            qty => qty.Value,
            value => Quantity.Create(value).Value)
    {
    }
}

