// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Fluent test data builder for creating <see cref="Address"/> Value Object instances in test suites.
/// </summary>
public sealed class AddressBuilder
{
    private string? _street = "Av. Winston Churchill #1099";
    private string? _city = "Santo Domingo";
    private string? _province = "Distrito Nacional";
    private Country _country = Country.Create("DO").Value;
    private PostalCode? _postalCode = PostalCode.Create("10101").Value;

    /// <summary>
    /// Sets the street component of the address.
    /// </summary>
    public AddressBuilder WithStreet(string? street)
    {
        _street = street;
        return this;
    }

    /// <summary>
    /// Sets the city component of the address.
    /// </summary>
    public AddressBuilder WithCity(string? city)
    {
        _city = city;
        return this;
    }

    /// <summary>
    /// Sets the province component of the address.
    /// </summary>
    public AddressBuilder WithProvince(string? province)
    {
        _province = province;
        return this;
    }

    /// <summary>
    /// Sets the country Value Object of the address.
    /// </summary>
    public AddressBuilder WithCountry(Country country)
    {
        _country = country;
        return this;
    }

    /// <summary>
    /// Sets the country from an ISO country code string.
    /// </summary>
    public AddressBuilder WithCountry(string? countryCode)
    {
        var result = Country.Create(countryCode);
        if (result.IsFailure)
        {
            throw new ArgumentException($"Invalid country code: {countryCode}", nameof(countryCode));
        }

        _country = result.Value;
        return this;
    }

    /// <summary>
    /// Sets the postal code Value Object of the address.
    /// </summary>
    public AddressBuilder WithPostalCode(PostalCode? postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    /// <summary>
    /// Sets the postal code from a raw string.
    /// </summary>
    public AddressBuilder WithPostalCode(string? postalCode)
    {
        if (postalCode is null)
        {
            _postalCode = null;
            return this;
        }

        var result = PostalCode.Create(postalCode);
        if (result.IsFailure)
        {
            throw new ArgumentException($"Invalid postal code: {postalCode}", nameof(postalCode));
        }

        _postalCode = result.Value;
        return this;
    }

    /// <summary>
    /// Clears the optional postal code so it is omitted from the address.
    /// </summary>
    public AddressBuilder WithoutPostalCode()
    {
        _postalCode = null;
        return this;
    }

    /// <summary>
    /// Builds and validates the <see cref="Address"/> instance, returning a <see cref="Result{Address}"/>.
    /// </summary>
    public Result<Address> BuildResult()
    {
        return Address.Create(_street, _city, _province, _country, _postalCode);
    }

    /// <summary>
    /// Builds the validated <see cref="Address"/> instance or throws <see cref="InvalidOperationException"/> if invalid.
    /// </summary>
    public Address Build()
    {
        var result = BuildResult();
        if (result.IsFailure)
        {
            throw new InvalidOperationException($"Failed to build valid Address: [{result.Error.Code}] {result.Error.Description}");
        }

        return result.Value;
    }
}
