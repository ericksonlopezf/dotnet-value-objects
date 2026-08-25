// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a structured physical address composed of street, city, province, country, and postal code.
/// </summary>
public sealed record Address : ValueObject
{
    /// <summary>
    /// Gets the street address component.
    /// </summary>
    public string Street { get; }

    /// <summary>
    /// Gets the city or municipality component.
    /// </summary>
    public string City { get; }

    /// <summary>
    /// Gets the state, province, or department component.
    /// </summary>
    public string Province { get; }

    /// <summary>
    /// Gets the ISO 3166-1 alpha-2 country component.
    /// </summary>
    public Country Country { get; }

    /// <summary>
    /// Gets the optional postal or ZIP code component.
    /// </summary>
    public PostalCode? PostalCode { get; }

    private Address(string street, string city, string province, Country country, PostalCode? postalCode)
    {
        Street = street;
        City = city;
        Province = province;
        Country = country;
        PostalCode = postalCode;
    }

    /// <summary>
    /// Creates a validated <see cref="Address"/> instance from normalized address components.
    /// </summary>
    /// <param name="street">The street address component.</param>
    /// <param name="city">The city name component.</param>
    /// <param name="province">The province or state component.</param>
    /// <param name="country">The country value object.</param>
    /// <param name="postalCode">The optional postal code value object.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated address, or a validation failure.</returns>
    public static Result<Address> Create(
        string? street,
        string? city,
        string? province,
        Country country,
        PostalCode? postalCode = null)
    {
        Result<string> streetResult = StringPipeline.RequiredString(
            street,
            "Address.Street",
            3,
            240,
            StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "Street address can contain letters, digits, spaces, and common punctuation.");

        if (streetResult.IsFailure)
            return Result<Address>.Failure(streetResult.Error);

        Result<string> cityResult = StringPipeline.RequiredString(
            city,
            "Address.City",
            1,
            120,
            StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "City can contain letters, digits, spaces, and common punctuation.");

        if (cityResult.IsFailure)
            return Result<Address>.Failure(cityResult.Error);

        Result<string> provinceResult = StringPipeline.RequiredString(
            province,
            "Address.Province",
            1,
            120,
            StringPipeline.NormalizeHumanName,
            StringPipeline.HumanNamePattern,
            "Province can contain letters, spaces, apostrophes, periods, or hyphens.");

        if (provinceResult.IsFailure)
            return Result<Address>.Failure(provinceResult.Error);

        return Result<Address>.Success(new Address(
            streetResult.Value,
            cityResult.Value,
            provinceResult.Value,
            country,
            postalCode));
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        string postalPart = PostalCode is not null ? $", {PostalCode}" : "";
        return $"{Street}, {City}, {Province}, {Country}{postalPart}";
    }
}



