// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="SalesChannelCode"/> Domain Primitive.
/// </summary>
public sealed class SalesChannelCodeTests
{
    [Theory]
    [InlineData("web_store", "WEB_STORE")]
    [InlineData("pos-kiosk-01", "POS-KIOSK-01")]
    public void SalesChannelCode_WhenValid_ShouldNormalizeToUpper(string input, string expected)
    {
        var result = SalesChannelCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
        result.Value.ToString().Should().Be(expected);
    }

    [Fact]
    public void SalesChannelCode_WhenInvalid_ShouldFail()
    {
        SalesChannelCode.Create(new string('a', 61)).Error.Code.Should().Be("SalesChannelCode.TooLong");
        var invalid = SalesChannelCode.Create("CHANNEL#999");
        invalid.Error.Code.Should().Be("SalesChannelCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Sales channel code must contain uppercase alphanumeric characters and standard separators.");
    }
}




