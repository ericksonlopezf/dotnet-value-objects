// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Peru;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Peru.UnitTests;

public sealed class CpeIdentifierTests
{
    [Fact]
    public void Create_FromComponents_Succeeds()
    {
        var result = CpeIdentifier.Create(CpeTypeCode.Factura, "f001", 1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Should().Be(CpeTypeCode.Factura);
        result.Value.Series.Should().Be("F001");
        result.Value.Number.Should().Be(1);
        result.Value.Canonical.Should().Be("01-F001-00000001");
        result.Value.ToString().Should().Be("01-F001-00000001");
    }

    [Theory]
    [InlineData("01-F001-00000001", "01", "F001", 1, "01-F001-00000001")]
    [InlineData("03-B001-1", "03", "B001", 1, "03-B001-00000001")]
    [InlineData("07-FC01-99999999", "07", "FC01", 99999999, "07-FC01-99999999")]
    [InlineData("  01-f001-100  ", "01", "F001", 100, "01-F001-00000100")]
    public void Create_FromValidString_ExtractsComponents(
        string input,
        string expectedTypeCode,
        string expectedSeries,
        int expectedNumber,
        string expectedCanonical)
    {
        var result = CpeIdentifier.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Type.Code.Should().Be(expectedTypeCode);
        result.Value.Series.Should().Be(expectedSeries);
        result.Value.Number.Should().Be(expectedNumber);
        result.Value.Canonical.Should().Be(expectedCanonical);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_RequiredSeries_ReturnsError(string? series)
    {
        var result = CpeIdentifier.Create(CpeTypeCode.Factura, series, 1);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeIdentifier.RequiredSeries");
    }

    [Theory]
    [InlineData("F01")]
    [InlineData("F0001")]
    [InlineData("F")]
    public void Create_InvalidSeriesLength_ReturnsError(string series)
    {
        var result = CpeIdentifier.Create(CpeTypeCode.Factura, series, 1);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeIdentifier.InvalidSeriesLength");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100_000_000)]
    public void Create_NumberOutOfRange_ReturnsError(int number)
    {
        var result = CpeIdentifier.Create(CpeTypeCode.Factura, "F001", number);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeIdentifier.NumberOutOfRange");
    }

    [Theory]
    [InlineData("01F00100000001")]
    [InlineData("01-F00100000001")]
    public void Create_InvalidFormat_ReturnsError(string input)
    {
        var result = CpeIdentifier.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeIdentifier.InvalidFormat");
    }

    [Fact]
    public void Create_InvalidTypeCodeInString_ReturnsError()
    {
        var result = CpeIdentifier.Create("99-F001-00000001");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeTypeCode.InvalidCode");
    }

    [Fact]
    public void Create_InvalidNumberInString_ReturnsError()
    {
        var result = CpeIdentifier.Create("01-F001-ABC");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("CpeIdentifier.InvalidNumber");
    }

    [Fact]
    public void CpeIdentifier_DefaultState_ComparisonOperators()
    {
        var c1 = CpeIdentifier.Create("01-F001-00000001").Value;
        var c2 = CpeIdentifier.Create("01-F001-00000002").Value;
        var c3 = CpeIdentifier.Create("01-F002-00000001").Value;
        var c4 = CpeIdentifier.Create("03-B001-00000001").Value;
        var c1Clone = CpeIdentifier.Create("01-F001-1").Value;

        (c1 < c2).Should().BeTrue();
        (c1 <= c2).Should().BeTrue();
        (c2 > c1).Should().BeTrue();
        (c2 >= c1).Should().BeTrue();

        (c1 < c3).Should().BeTrue();
        (c1 < c4).Should().BeTrue();

        (c1 < c1Clone).Should().BeFalse();
        (c1 > c1Clone).Should().BeFalse();
        (c1 <= c1Clone).Should().BeTrue();
        (c1 >= c1Clone).Should().BeTrue();
        c1.CompareTo(c2).Should().BeNegative();
        c2.CompareTo(c1).Should().BePositive();
        c1.CompareTo(c1Clone).Should().Be(0);
    }

    [Fact]
    public void CpeIdentifier_DefaultState_ParseAndTryParse()
    {
        var validStr = "01-F001-00000001";
        var parsed1 = CpeIdentifier.Parse(validStr, CultureInfo.InvariantCulture);
        parsed1.Canonical.Should().Be(validStr);

        var parsed2 = CpeIdentifier.Parse(validStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Canonical.Should().Be(validStr);

        CpeIdentifier.TryParse(validStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Canonical.Should().Be(validStr);

        CpeIdentifier.TryParse(validStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Canonical.Should().Be(validStr);

        Action invalidParseStr = () => CpeIdentifier.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid CpeIdentifier: 'invalid'.");

        Action invalidParseSpan = () => CpeIdentifier.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid CpeIdentifier: 'invalid'.");

        CpeIdentifier.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(CpeIdentifier));

        CpeIdentifier.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(CpeIdentifier));

        CpeIdentifier.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(CpeIdentifier));
    }
}




