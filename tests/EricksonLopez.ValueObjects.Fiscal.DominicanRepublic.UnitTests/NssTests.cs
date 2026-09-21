// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class NssTests
{
    [Fact]
    public void Create_ValidNss_Succeeds()
    {
        // 12345678: 1*1=1, 2*2=4, 3*1=3, 4*2=8, 5*1=5, 6*2=12(1+2=3), 7*1=7, 8*2=16(1+6=7)
        // sum = 1+4+3+8+5+3+7+7 = 38. (10 - (38%10))%10 = 2 -> 123456782
        var result = Nss.Create("123456782");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("123456782");
        result.Value.Formatted.Should().Be("123-456782");
    }

    [Fact]
    public void Create_ValidFormattedNss_StripsHyphenAndSucceeds()
    {
        var result = Nss.Create("123-456782");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("123456782");
    }

    [Fact]
    public void Create_InvalidCheckDigit_ReturnsFailure()
    {
        var result = Nss.Create("123456789");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nss.InvalidCheckDigit");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = Nss.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nss.Required");
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("1234567890")]
    public void Create_InvalidLength_ReturnsInvalidLengthError(string invalid)
    {
        var result = Nss.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Nss.InvalidLength");
    }

    [Fact]
    public void Nss_Equality_AndToString()
    {
        var a = Nss.Create("123-456782").Value;
        var b = Nss.Create("123456782").Value;

        (a == b).Should().BeTrue();
        a.ToString().Should().Be("123456782");

        var tooLong = Nss.Create(new string('1', 35));
        tooLong.IsFailure.Should().BeTrue();
        tooLong.Error.Code.Should().Be("Nss.InvalidLength");
    }

    [Fact]
    public void ValidateNss_DirectCalls()
    {
        NssChecksum.ValidateNss("123").Should().BeFalse();
        NssChecksum.ValidateNss("1234567A2").Should().BeFalse();
        NssChecksum.ValidateNss("123456782").Should().BeTrue();

        var reconstituted = Nss.Reconstitute("123456782");
        reconstituted.Value.Should().Be("123456782");

        var hydrated = Nss.Hydrate("123456782");
        hydrated.Value.Should().Be("123456782");
    }
}
