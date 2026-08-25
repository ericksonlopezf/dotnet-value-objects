// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ReceiptNumber"/> Domain Primitive.
/// </summary>
public sealed class ReceiptNumberTests
{
    [Theory]
    [InlineData("rcpt-00124", "RCPT-00124")]
    [InlineData("pos/2026/099", "POS/2026/099")]
    public void ReceiptNumber_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = ReceiptNumber.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void ReceiptNumber_WhenInvalid_ShouldFail()
    {
        ReceiptNumber.Create(new string('a', 81)).Error.Code.Should().Be("ReceiptNumber.TooLong");
        var invalid = ReceiptNumber.Create("RCPT#999");
        invalid.Error.Code.Should().Be("ReceiptNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Receipt number must contain uppercase alphanumeric characters and standard separators.");
    }
}




