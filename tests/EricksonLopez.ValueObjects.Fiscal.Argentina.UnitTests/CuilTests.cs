// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class CuilTests
{
    [Theory]
    [InlineData("20-12345678-6")]
    [InlineData("23-12345678-5")]
    [InlineData("24-12345678-1")]
    [InlineData("27-23456789-1")]
    public void Create_ValidIndividualPrefixes_Succeeds(string input)
    {
        var result = Cuil.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Length.Should().Be(11);
        result.Value.AsCuit.Value.Should().Be(result.Value.Value);
        result.Value.Formatted.Should().Be(result.Value.AsCuit.Formatted);
        result.Value.ToString().Should().Be(result.Value.Formatted);
    }

    [Theory]
    [InlineData("30-65432109-0")]
    [InlineData("33-65432109-9")]
    [InlineData("34-65432109-6")]
    public void Create_CompanyPrefix_ReturnsInvalidPrefixError(string input)
    {
        var result = Cuil.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuil.InvalidPrefix");
        result.Error.Description.Should().Contain("individual persons");
    }

    [Fact]
    public void Create_InvalidCuitFormat_PropagatesCuitFailure()
    {
        var result = Cuil.Create("20-12345678-0"); // Wrong DV
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Cuit.InvalidVerificationDigit");
    }

    [Fact]
    public void Cuil_ParseAndTryParse_StringAndSpan()
    {
        var validStr = "20-12345678-6";
        var parsed1 = Cuil.Parse(validStr, System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be("20123456786");

        var parsed2 = Cuil.Parse(validStr.AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be("20123456786");

        Cuil.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be("20123456786");

        Cuil.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be("20123456786");

        Action invalidParseStr = () => Cuil.Parse("30-65432109-0", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CUIL: '30-65432109-0'.");

        Action invalidParseSpan = () => Cuil.Parse("30-65432109-0".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CUIL: '30-65432109-0'.");

        Cuil.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(Cuil));

        Cuil.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(Cuil));

        Cuil.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(Cuil));
    }

    [Fact]
    public void Cuil_ParseAndTryParse_Utf8()
    {
        byte[] validUtf8 = Encoding.UTF8.GetBytes("20-12345678-6");
        var parsed = Cuil.Parse(validUtf8, System.Globalization.CultureInfo.InvariantCulture);
        parsed.Value.Should().Be("20123456786");

        Cuil.TryParse(validUtf8, null, out var tryRes).Should().BeTrue();
        tryRes.Value.Should().Be("20123456786");

        byte[] invalidUtf8 = Encoding.UTF8.GetBytes("30-65432109-0");
        Action invalidParseUtf8 = () => Cuil.Parse(invalidUtf8, System.Globalization.CultureInfo.InvariantCulture);
        invalidParseUtf8.Should().Throw<FormatException>().WithMessage("Invalid UTF-8 CUIL representation.");

        Cuil.TryParse(invalidUtf8, null, out var tryFail).Should().BeFalse();
        tryFail.Should().Be(default(Cuil));

        byte[] brokenUtf8 = [0xFF, 0xFE, 0xFD];
        Cuil.TryParse(brokenUtf8, null, out var tryBroken).Should().BeFalse();
        tryBroken.Should().Be(default(Cuil));
    }

    [Fact]
    public void Cuil_ComparisonsAndOperators_Exhaustive()
    {
        var a = Cuil.Create("20-12345678-6").Value;
        var aCopy = Cuil.Create("20-12345678-6").Value;
        var b = Cuil.Create("27-23456789-1").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




