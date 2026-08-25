// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PositionTitle"/> Domain Primitive.
/// </summary>
public sealed class PositionTitleTests
{
    [Fact]
    public void PositionTitle_WhenValid_ShouldCollapseWhitespace()
    {
        var result = PositionTitle.Create("   Senior   Software   Architect  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Senior Software Architect");
        result.Value.ToString().Should().Be("Senior Software Architect");
    }

    [Fact]
    public void PositionTitle_WhenInvalid_ShouldFail()
    {
        PositionTitle.Create(new string('a', 121)).Error.Code.Should().Be("PositionTitle.TooLong");
        var invalid = PositionTitle.Create("Position<title>");
        invalid.Error.Code.Should().Be("PositionTitle.InvalidFormat");
        invalid.Error.Description.Should().Be("Position title must contain valid letters, spaces, and punctuation.");
    }
}




