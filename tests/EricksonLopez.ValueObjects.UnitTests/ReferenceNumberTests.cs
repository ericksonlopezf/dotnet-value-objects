// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ReferenceNumber"/> Domain Primitive.
/// </summary>
public sealed class ReferenceNumberTests
{
    [Fact]
    public void ReferenceNumber_Valid_NormalizesUppercase()
    {
        var reference = ReferenceNumber.Create("ref/9876_b").Value;
        reference.Value.Should().Be("REF/9876_B");
        reference.ToString().Should().Be("REF/9876_B");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("REF#999")]
    public void ReferenceNumber_Invalid_ShouldFail(string? invalid)
    {
        var result = ReferenceNumber.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("ReferenceNumber.Required");
        else result.Error.Code.Should().Be("ReferenceNumber.InvalidFormat");
    }

    [Fact]
    public void ReferenceNumber_Invalid_ShouldReturnSpecificErrors()
    {
        var invalid = ReferenceNumber.Create("REF#999");
        invalid.Error.Code.Should().Be("ReferenceNumber.InvalidFormat");
        invalid.Error.Description.Should().Be("Reference number must start with an alphanumeric character and contain only letters, digits, periods, underscores, slashes, or hyphens.");

        ReferenceNumber.Create(new string('A', 81)).Error.Code.Should().Be("ReferenceNumber.TooLong");
    }
}




