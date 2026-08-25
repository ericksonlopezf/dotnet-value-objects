// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="SerialNumber"/> Domain Primitive.
/// </summary>
public sealed class SerialNumberTests
{
    [Theory]
    [InlineData("sn-99281-ab", "SN-99281-AB")]
    [InlineData("1234567890", "1234567890")]
    public void SerialNumber_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = SerialNumber.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void SerialNumber_WhenInvalid_ShouldFail()
    {
        SerialNumber.Create(new string('a', 81)).Error.Code.Should().Be("SerialNumber.TooLong");
        var invalid = SerialNumber.Create("SN#999");
        invalid.Error.Code.Should().Be("SerialNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Serial number must contain uppercase alphanumeric characters and standard separators.");
    }
}




