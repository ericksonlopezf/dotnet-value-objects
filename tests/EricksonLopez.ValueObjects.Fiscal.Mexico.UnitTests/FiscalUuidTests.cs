// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class FiscalUuidTests
{
    private const string ValidUuidStr = "a3bb189e-8bf9-4888-9912-ace4e6543002";
    private static readonly Guid ValidGuid = Guid.Parse(ValidUuidStr);

    [Fact]
    public void Create_FromValidGuid_Succeeds()
    {
        var result = FiscalUuid.Create(ValidGuid);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidGuid);
        result.Value.Formatted.Should().Be(ValidUuidStr.ToUpperInvariant());
        result.Value.ToString().Should().Be(ValidUuidStr.ToUpperInvariant());
    }

    [Fact]
    public void Create_FromEmptyGuid_ReturnsEmptyError()
    {
        var result = FiscalUuid.Create(Guid.Empty);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FiscalUuid.Empty");
    }

    [Theory]
    [InlineData("a3bb189e-8bf9-4888-9912-ace4e6543002")]
    [InlineData("A3BB189E-8BF9-4888-9912-ACE4E6543002")]
    [InlineData("  A3BB189E-8BF9-4888-9912-ACE4E6543002  ")]
    public void Create_FromValidString_Succeeds(string input)
    {
        var result = FiscalUuid.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(ValidGuid);
        result.Value.Formatted.Should().Be(ValidUuidStr.ToUpperInvariant());
    }

    [Theory]
    [InlineData("not-a-guid")]
    [InlineData("00000000-0000-0000-0000-000000000000")] // Guid.Empty string -> fails on empty check inside Create(parsedGuid)
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidString_ReturnsError(string? input)
    {
        var result = FiscalUuid.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Match(c => c == "FiscalUuid.InvalidFormat" || c == "FiscalUuid.Empty");
    }

    [Fact]
    public void FiscalUuid_DefaultState_ComparisonOperators()
    {
        var g1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var g2 = Guid.Parse("00000000-0000-0000-0000-000000000002");
        var u1 = FiscalUuid.Create(g1).Value;
        var u2 = FiscalUuid.Create(g2).Value;
        var u1Clone = FiscalUuid.Create(g1).Value;

        (u1 < u2).Should().BeTrue();
        (u1 <= u2).Should().BeTrue();
        (u2 > u1).Should().BeTrue();
        (u2 >= u1).Should().BeTrue();

        (u1 < u1Clone).Should().BeFalse();
        (u1 > u1Clone).Should().BeFalse();
        (u1 <= u1Clone).Should().BeTrue();
        (u1 >= u1Clone).Should().BeTrue();
        u1.CompareTo(u2).Should().BeNegative();
        u2.CompareTo(u1).Should().BePositive();
        u1.CompareTo(u1Clone).Should().Be(0);
    }

    [Fact]
    public void FiscalUuid_DefaultState_ParseAndTryParse()
    {
        var parsed1 = FiscalUuid.Parse(ValidUuidStr, CultureInfo.InvariantCulture);
        parsed1.Value.Should().Be(ValidGuid);

        var parsed2 = FiscalUuid.Parse(ValidUuidStr.AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Value.Should().Be(ValidGuid);

        FiscalUuid.TryParse(ValidUuidStr, null, out var tryRes1).Should().BeTrue();
        tryRes1.Value.Should().Be(ValidGuid);

        FiscalUuid.TryParse(ValidUuidStr.AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Value.Should().Be(ValidGuid);

        Action invalidParseStr = () => FiscalUuid.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid FiscalUuid: 'invalid'.");

        Action invalidParseSpan = () => FiscalUuid.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid FiscalUuid: 'invalid'.");

        FiscalUuid.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(FiscalUuid));

        FiscalUuid.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(FiscalUuid));

        FiscalUuid.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(FiscalUuid));
    }
}




