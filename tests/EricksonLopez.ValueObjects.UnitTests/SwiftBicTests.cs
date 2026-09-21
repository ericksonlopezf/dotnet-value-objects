// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class SwiftBicTests
{
    [Fact]
    public void Create_Valid8CharBic_Succeeds()
    {
        var result = SwiftBic.Create("BPDODO22");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("BPDODO22");
        result.Value.BankCode.Should().Be("BPDO");
        result.Value.CountryCode.Should().Be("DO");
        result.Value.LocationCode.Should().Be("22");
        result.Value.BranchCode.Should().BeNull();
        result.Value.IsHeadOffice.Should().BeTrue();
    }

    [Fact]
    public void Create_Valid11CharBic_Succeeds()
    {
        var result = SwiftBic.Create("BPDODO22XXX");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("BPDODO22XXX");
        result.Value.BranchCode.Should().Be("XXX");
        result.Value.IsHeadOffice.Should().BeFalse();
    }

    [Theory]
    [InlineData("1234DO22")] // Numeric bank code
    public void Create_InvalidBankCode_ReturnsBankCodeError(string invalid)
    {
        var result = SwiftBic.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.InvalidBankCode");
    }

    [Theory]
    [InlineData("BPDO1222")] // Numeric country code
    public void Create_InvalidCountryCode_ReturnsCountryCodeError(string invalid)
    {
        var result = SwiftBic.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.InvalidCountryCode");
    }

    [Theory]
    [InlineData("BPDO")]
    [InlineData("BPDODO2")]
    [InlineData("BPDODO22XXXX")] // 12 chars
    public void Create_InvalidLength_ReturnsLengthError(string invalid)
    {
        var result = SwiftBic.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.InvalidLength");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = SwiftBic.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.Required");
    }

    [Fact]
    public void Create_InvalidLocationCode_ReturnsError()
    {
        var result = SwiftBic.Create("BPDODO--XXX");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.InvalidLocationCode");
    }

    [Fact]
    public void Create_InvalidBranchCode_ReturnsError()
    {
        var result = SwiftBic.Create("BPDODO22X-X");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("SwiftBic.InvalidBranchCode");
    }

    [Fact]
    public void Reconstitute_And_Hydrate_WorkCorrectly()
    {
        SwiftBic.Reconstitute("BPDODO22XXX").Value.Should().Be("BPDODO22XXX");
        SwiftBic.Hydrate("BPDODO22XXX").Value.Should().Be("BPDODO22XXX");
    }
}
