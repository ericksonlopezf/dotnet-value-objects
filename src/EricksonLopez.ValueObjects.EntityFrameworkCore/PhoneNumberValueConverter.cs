// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="PhoneNumber"/>.
/// </summary>
public sealed class PhoneNumberValueConverter : ValueConverter<PhoneNumber, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhoneNumberValueConverter"/> class.
    /// </summary>
    public PhoneNumberValueConverter()
        : base(
            phone => phone.Value,
            value => PhoneNumber.Create(value).Value)
    {
    }
}

