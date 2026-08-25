// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Description"/> Domain Primitive.
/// </summary>
public sealed class DescriptionTests
{
    [Fact]
    public void Description_Valid_CollapsesWhitespace()
    {
        var desc = Description.Create("  Multi   space   description  ").Value;
        desc.Value.Should().Be("Multi space description");
        desc.ToString().Should().Be("Multi space description");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Description_Invalid_ShouldFail(string? invalid)
    {
        var result = Description.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("Description.Required");
    }

    [Fact]
    public void Description_TooLong_ShouldFail()
    {
        Description.Create(new string('a', 1001)).Error.Code.Should().Be("Description.TooLong");
    }
}




