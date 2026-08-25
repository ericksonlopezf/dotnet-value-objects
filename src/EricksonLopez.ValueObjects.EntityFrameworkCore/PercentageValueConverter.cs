// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="Percentage"/>.
/// </summary>
public sealed class PercentageValueConverter : ValueConverter<Percentage, decimal>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PercentageValueConverter"/> class.
    /// </summary>
    public PercentageValueConverter()
        : base(
            percentage => percentage.Value,
            value => Percentage.Create(value).Value)
    {
    }
}
