// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.EntityFrameworkCore;

using System;
using EricksonLopez.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

/// <summary>
/// Provides an Entity Framework Core <see cref="ValueConverter{TModel, TProvider}"/> for <see cref="PostalCode"/>.
/// </summary>
public sealed class PostalCodeValueConverter : ValueConverter<PostalCode, string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostalCodeValueConverter"/> class.
    /// </summary>
    public PostalCodeValueConverter()
        : base(
            postalCode => postalCode.Value,
            value => PostalCode.Create(value).Value)
    {
    }
}
