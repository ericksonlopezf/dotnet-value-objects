// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class VoucherLetterTests
{
    [Theory]
    [InlineData('A', 'A')]
    [InlineData('a', 'A')]
    [InlineData('B', 'B')]
    [InlineData('b', 'B')]
    [InlineData('C', 'C')]
    [InlineData('c', 'C')]
    [InlineData('E', 'E')]
    [InlineData('e', 'E')]
    [InlineData('M', 'M')]
    [InlineData('m', 'M')]
    [InlineData('T', 'T')]
    [InlineData('t', 'T')]
    [InlineData('R', 'R')]
    [InlineData('r', 'R')]
    public void Create_ValidLetters_Succeeds(char input, char expected)
    {
        var result = VoucherLetter.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Letter.Should().Be(expected);
        result.Value.ToString().Should().Be(expected.ToString());
    }

    [Fact]
    public void KnownInstances_DefaultState_MatchExpectedLetters()
    {
        VoucherLetter.A.Letter.Should().Be('A');
        VoucherLetter.B.Letter.Should().Be('B');
        VoucherLetter.C.Letter.Should().Be('C');
        VoucherLetter.E.Letter.Should().Be('E');
        VoucherLetter.M.Letter.Should().Be('M');
        VoucherLetter.T.Letter.Should().Be('T');
        VoucherLetter.R.Letter.Should().Be('R');
    }

    [Theory]
    [InlineData('Z')]
    [InlineData('1')]
    [InlineData('X')]
    public void Create_InvalidLetter_ReturnsError(char invalid)
    {
        var result = VoucherLetter.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherLetter.InvalidLetter");
    }

    [Theory]
    [InlineData("AA")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidLength_ReturnsError(string invalid)
    {
        var result = VoucherLetter.Create(invalid);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherLetter.InvalidLength");
    }

    [Fact]
    public void VoucherLetter_DefaultState_ParseAndTryParse()
    {
        var parsed1 = VoucherLetter.Parse("A", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Letter.Should().Be('A');

        var parsed2 = VoucherLetter.Parse("A".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Letter.Should().Be('A');

        VoucherLetter.TryParse("A", null, out var tryRes1).Should().BeTrue();
        tryRes1.Letter.Should().Be('A');

        VoucherLetter.TryParse("A".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Letter.Should().Be('A');

        Action invalidParseStr = () => VoucherLetter.Parse("Z", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid VoucherLetter: 'Z'.");

        Action invalidParseSpan = () => VoucherLetter.Parse("Z".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid VoucherLetter: 'Z'.");

        VoucherLetter.TryParse("Z", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(VoucherLetter));

        VoucherLetter.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(VoucherLetter));

        VoucherLetter.TryParse("Z".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(VoucherLetter));

        VoucherLetter.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void VoucherLetter_ComparisonsAndOperators_Exhaustive()
    {
        var a = VoucherLetter.A;
        var aCopy = VoucherLetter.Create('A').Value;
        var b = VoucherLetter.B;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




