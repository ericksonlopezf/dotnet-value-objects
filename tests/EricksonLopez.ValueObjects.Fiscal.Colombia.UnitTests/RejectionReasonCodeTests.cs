// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Colombia;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia.UnitTests;

public sealed class RejectionReasonCodeTests
{
    [Theory]
    [InlineData("01", "Documento con inconsistencias")]
    [InlineData("02", "Mercancía no entregada totalmente")]
    [InlineData("03", "Mercancía no entregada parcialmente")]
    [InlineData("04", "Servicio no prestado")]
    [InlineData("  01  ", "Documento con inconsistencias")]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(string input, string expectedDescription)
    {
        var result = RejectionReasonCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        RejectionReasonCode.Inconsistencies.Code.Should().Be("01");
        RejectionReasonCode.GoodsNotDeliveredTotally.Code.Should().Be("02");
        RejectionReasonCode.GoodsNotDeliveredPartially.Code.Should().Be("03");
        RejectionReasonCode.ServiceNotRendered.Code.Should().Be("04");
    }

    [Theory]
    [InlineData("05")]
    [InlineData("00")]
    [InlineData("99")]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidCode_ReturnsError(string? input)
    {
        var result = RejectionReasonCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("RejectionReasonCode.InvalidCode");
    }

    [Fact]
    public void RejectionReasonCode_DefaultState_Equality()
    {
        var r1 = RejectionReasonCode.Inconsistencies;
        var r2 = RejectionReasonCode.Create("01").Value;
        var rDiff = RejectionReasonCode.ServiceNotRendered;

        (r1 == r2).Should().BeTrue();
        (r1 != rDiff).Should().BeTrue();
        r1.Equals(r2).Should().BeTrue();
        r1.Equals((object)r2).Should().BeTrue();
        r1.Equals(rDiff).Should().BeFalse();
        r1.GetHashCode().Should().Be(r2.GetHashCode());
    }

    [Fact]
    public void RejectionReasonCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = RejectionReasonCode.Parse("01", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("01");

        var parsed2 = RejectionReasonCode.Parse("01".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("01");

        RejectionReasonCode.TryParse("01", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("01");

        RejectionReasonCode.TryParse("01".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("01");

        Action invalidParseStr = () => RejectionReasonCode.Parse("05", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid RADIAN rejection reason code: '05'.");

        Action invalidParseSpan = () => RejectionReasonCode.Parse("05".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid RADIAN rejection reason code: '05'.");

        RejectionReasonCode.TryParse("05", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(RejectionReasonCode));

        RejectionReasonCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(RejectionReasonCode));

        RejectionReasonCode.TryParse("05".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(RejectionReasonCode));
    }
}




