// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="TimeZoneCode"/> Value Object.
/// </summary>
public sealed class TimeZoneCodeTests
{
    [Theory]
    [InlineData("America/Santo_Domingo")]
    [InlineData("UTC")]
    [InlineData("Eastern Standard Time")]
    public void TimeZoneCode_WhenValid_ShouldSucceed(string input)
    {
        var result = TimeZoneCode.Create(input);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(input);
        result.Value.ToString().Should().Be(input);
    }

    [Fact]
    public void TimeZoneCode_WhenInvalid_ShouldFailWithSpecificErrors()
    {
        TimeZoneCode.Create("a").Error.Code.Should().Be("TimeZoneCode.TooShort");
        TimeZoneCode.Create(new string('a', 121)).Error.Code.Should().Be("TimeZoneCode.TooLong");
        var invalid = TimeZoneCode.Create("Invalid@TZ#");
        invalid.Error.Code.Should().Be("TimeZoneCode.InvalidFormat");
        invalid.Error.Description.Should().Be("Time zone must be a valid IANA or Windows time zone identifier.");
    }
}




