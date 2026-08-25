// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Email"/> Value Object.
/// </summary>
public sealed class EmailTests
{
    [Fact]
    public void Email_ValidEmail_NormalizesToLowercaseAndExtractsParts()
    {
        var result = Email.Create("  USER.Name+tag@Example.COM  ");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("user.name+tag@example.com");
        result.Value.Domain.Should().Be("example.com");
        result.Value.LocalPart.Should().Be("user.name+tag");
        result.Value.ToString().Should().Be("user.name+tag@example.com");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("nodomain")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    public void Email_InvalidEmail_ShouldFail(string? invalid)
    {
        var result = Email.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("Email.Required");
        else result.Error.Code.Should().Be("Email.InvalidFormat");
    }

    [Fact]
    public void Email_TooLong_ShouldFail()
    {
        var valid320 = new string('a', 64) + "@" + new string('b', 251) + ".com"; // 64 + 1 + 251 + 4 = 320
        valid320.Length.Should().Be(320);
        Email.Create(valid320).IsSuccess.Should().BeTrue();

        var longEmail = new string('a', 64) + "@" + new string('b', 252) + ".com"; // 321 chars
        var res = Email.Create(longEmail);
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Email.TooLong");
    }

    [Fact]
    public void Email_DefaultStruct_AndOperators_Exhaustive()
    {
        default(Email).LocalPart.Should().Be(string.Empty);
        default(Email).Domain.Should().Be(string.Empty);
        default(Email).ToString().Should().Be(string.Empty);

        var a = Email.Create("alpha@example.com").Value;
        var aCopy = Email.Create("ALPHA@EXAMPLE.COM").Value;
        var b = Email.Create("beta@example.com").Value;

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);

        ((IComparable)a).CompareTo((object)a).Should().Be(0);
        ((IComparable)a).CompareTo((object)b).Should().BeNegative();

        Action invalidObj = () => a.CompareTo("not-an-email");
        invalidObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not an Email*");

        Action nullObj = () => ((IComparable)a).CompareTo(null);
        nullObj.Should().Throw<ArgumentException>()
            .WithMessage("*Object is not an Email*");
    }

    [Fact]
    public void Email_WithMultipleAtSymbols_UsesLastIndexOfAsDomainSeparator()
    {
        var email = Email.Create("\"john@doe\"@example.com").Value;
        email.LocalPart.Should().Be("\"john@doe\"");
        email.Domain.Should().Be("example.com");
        email.Masked().Should().Be("\"***@example.com");
    }

    [Fact]
    public void Email_Masked_RedactsLocalPart()
    {
        var e1 = Email.Create("johndoe@example.com").Value;
        e1.Masked().Should().Be("j***@example.com");

        var eShort = Email.Create("a@example.com").Value;
        eShort.Masked().Should().Be("***@example.com");

        default(Email).Masked().Should().BeEmpty();
    }

    [Fact]
    public void Parsing_StringAndSpan_ParsesOrThrows()
    {
        var e = Email.Parse("user@example.com", CultureInfo.InvariantCulture);
        e.Value.Should().Be("user@example.com");

        Email.Parse("  USER@DOMAIN.ORG  ", CultureInfo.InvariantCulture).Value.Should().Be("user@domain.org");
        Email.Parse("admin@test.com".AsSpan(), CultureInfo.InvariantCulture).Value.Should().Be("admin@test.com");

        Action nullAct = () => Email.Parse((string)null!, CultureInfo.InvariantCulture);
        nullAct.Should().Throw<FormatException>();

        Action invalidFormat = () => Email.Parse("invalid-email", CultureInfo.InvariantCulture);
        invalidFormat.Should().Throw<FormatException>()
            .WithMessage("Invalid email format: 'invalid-email'.");

        Action invalidSpan = () => Email.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidSpan.Should().Throw<FormatException>();
    }

    [Fact]
    public void TryParse_StringAndSpan_ReturnsSuccessOrFailure()
    {
        Email.TryParse("user@example.com", null, out var r1).Should().BeTrue();
        r1.Value.Should().Be("user@example.com");

        Email.TryParse(null, null, out var rNull).Should().BeFalse();
        rNull.Should().Be(default);

        Email.TryParse("invalid-email", null, out var rInvalid).Should().BeFalse();

        Email.TryParse("user@test.org".AsSpan(), null, out var rSpan).Should().BeTrue();
        rSpan.Value.Should().Be("user@test.org");

        Email.TryParse("invalid".AsSpan(), null, out var rSpanInvalid).Should().BeFalse();
    }
}




