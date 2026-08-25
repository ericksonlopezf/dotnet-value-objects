// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Address"/> Value Object.
/// </summary>
public sealed class AddressTests
{
    [Fact]
    public void Create_WhenAllComponentsProvided_FormatsCorrectly()
    {
        var country = Country.Create("DO").Value;
        var postal = PostalCode.Create("10101").Value;
        var address = Address.Create("Av. Winston Churchill #1099", "Santo Domingo", "Distrito Nacional", country, postal).Value;

        address.Street.Should().Be("Av. Winston Churchill #1099");
        address.City.Should().Be("Santo Domingo");
        address.Province.Should().Be("Distrito Nacional");
        address.Country.Value.Should().Be("DO");
        address.PostalCode!.Value.Should().Be("10101");
        address.ToString().Should().Be("Av. Winston Churchill #1099, Santo Domingo, Distrito Nacional, DO, 10101");
    }

    [Fact]
    public void Create_WhenPostalCodeOmitted_Succeeds()
    {
        var country = Country.Create("DO").Value;
        var result = Address.Create("Calle 1", "Santiago", "Santiago", country);

        result.IsSuccess.Should().BeTrue();
        result.Value.PostalCode.Should().BeNull();
        result.Value.ToString().Should().Be("Calle 1, Santiago, Santiago, DO");
    }

    [Fact]
    public void Create_WhenFieldsAreInvalid_ReturnsSpecificErrorCodes()
    {
        var country = Country.Create("DO").Value;

        var nullStreet = Address.Create(null, "City", "Province", country);
        nullStreet.IsFailure.Should().BeTrue();
        nullStreet.Error.Code.Should().Be("Address.Street.Required");

        var shortStreet = Address.Create("AB", "City", "Province", country);
        shortStreet.IsFailure.Should().BeTrue();
        shortStreet.Error.Code.Should().Be("Address.Street.TooShort");

        var longStreet = Address.Create(new string('a', 241), "City", "Province", country);
        longStreet.IsFailure.Should().BeTrue();
        longStreet.Error.Code.Should().Be("Address.Street.TooLong");

        var nullCity = Address.Create("Street", null, "Province", country);
        nullCity.IsFailure.Should().BeTrue();
        nullCity.Error.Code.Should().Be("Address.City.Required");

        var longCity = Address.Create("Street", new string('a', 121), "Province", country);
        longCity.IsFailure.Should().BeTrue();
        longCity.Error.Code.Should().Be("Address.City.TooLong");

        var nullProvince = Address.Create("Street", "City", null, country);
        nullProvince.IsFailure.Should().BeTrue();
        nullProvince.Error.Code.Should().Be("Address.Province.Required");

        var longProvince = Address.Create("Street", "City", new string('a', 121), country);
        longProvince.IsFailure.Should().BeTrue();
        longProvince.Error.Code.Should().Be("Address.Province.TooLong");

        var invalidStreetPattern = Address.Create("Street<script>", "City", "Province", country);
        invalidStreetPattern.IsFailure.Should().BeTrue();
        invalidStreetPattern.Error.Code.Should().Be("Address.Street.InvalidFormat");
        invalidStreetPattern.Error.Description.Should().Be("Street address can contain letters, digits, spaces, and common punctuation.");

        var invalidCityPattern = Address.Create("Street", "City<script>", "Province", country);
        invalidCityPattern.IsFailure.Should().BeTrue();
        invalidCityPattern.Error.Code.Should().Be("Address.City.InvalidFormat");
        invalidCityPattern.Error.Description.Should().Be("City can contain letters, digits, spaces, and common punctuation.");

        var invalidProvincePattern = Address.Create("Street", "City", "Province123", country);
        invalidProvincePattern.IsFailure.Should().BeTrue();
        invalidProvincePattern.Error.Code.Should().Be("Address.Province.InvalidFormat");
        invalidProvincePattern.Error.Description.Should().Be("Province can contain letters, spaces, apostrophes, periods, or hyphens.");
    }

    [Fact]
    public void Create_WhenStreetContainsExcessiveWhitespace_NormalizesProperly()
    {
        var country = Country.Create("DO").Value;
        var result = Address.Create("Calle   Principal   123", "Ciudad", "Provincia", country);
        result.IsSuccess.Should().BeTrue();
        result.Value.Street.Should().Be("Calle Principal 123");
    }

    [Fact]
    public void EqualityContract_WhenValidAddresses_SatisfiesContract()
    {
        var country = Country.Create("DO").Value;
        var postal = PostalCode.Create("10101").Value;
        var addr1 = Address.Create("Calle 1", "Santiago", "Santiago", country, postal).Value;
        var addr1Copy = Address.Create("Calle 1", "Santiago", "Santiago", country, postal).Value;
        var addr2 = Address.Create("Calle 2", "Santiago", "Santiago", country, postal).Value;

        addr1.ShouldSatisfyEqualityContract(addr1Copy, addr2, (a, b) => a == b, (a, b) => a != b);
    }

    [Fact]
    public void AddressBuilder_WhenConstructedFluently_ProducesExpectedInstance()
    {
        var address = new AddressBuilder()
            .WithStreet("Av. Gustavo Mejia Ricart #54")
            .WithCity("Santo Domingo")
            .WithProvince("Distrito Nacional")
            .WithCountry("DO")
            .WithPostalCode("10125")
            .Build();

        address.Street.Should().Be("Av. Gustavo Mejia Ricart #54");
        address.City.Should().Be("Santo Domingo");
        address.Province.Should().Be("Distrito Nacional");
        address.Country.Value.Should().Be("DO");
        address.PostalCode!.Value.Should().Be("10125");
    }

    [Fact]
    public void AddressBuilder_WhenPostalCodeCleared_ProducesAddressWithoutPostalCode()
    {
        var address = new AddressBuilder()
            .WithStreet("Calle Las Damas #1")
            .WithCity("Santo Domingo")
            .WithProvince("Distrito Nacional")
            .WithoutPostalCode()
            .Build();

        address.PostalCode.Should().BeNull();
        address.ToString().Should().Be("Calle Las Damas #1, Santo Domingo, Distrito Nacional, DO");
    }

    [Fact]
    public void AddressBuilder_WhenInvalidValuesProvided_ThrowsAppropriateExceptions()
    {
        var builder = new AddressBuilder();
        Action invalidCountry = () => builder.WithCountry("INVALID");
        invalidCountry.Should().Throw<ArgumentException>()
            .WithParameterName("countryCode");

        Action invalidPostal = () => builder.WithPostalCode("NOT_A_POSTAL_CODE_BECAUSE_TOO_LONG_1234567890");
        invalidPostal.Should().Throw<ArgumentException>()
            .WithParameterName("postalCode");

        builder.WithStreet(null);
        Action buildInvalid = () => builder.Build();
        buildInvalid.Should().Throw<InvalidOperationException>()
            .WithMessage("*Failed to build valid Address*");
    }
}




