// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Subject"/> Domain Primitive.
/// </summary>
public sealed class SubjectTests
{
    [Fact]
    public void Subject_WhenValid_ShouldCollapseWhitespace()
    {
        var result = Subject.Create("  Monthly   Billing   Statement   Report  ");
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("Monthly Billing Statement Report");
        result.Value.ToString().Should().Be("Monthly Billing Statement Report");
    }

    [Fact]
    public void Subject_WhenInvalid_ShouldFail()
    {
        Subject.Create(new string('a', 251)).Error.Code.Should().Be("Subject.TooLong");
        var invalid = Subject.Create("Subject\0Invalid");
        invalid.Error.Code.Should().Be("Subject.ControlCharacters");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Subject_WhenBlank_ShouldFail(string? invalid)
    {
        var subj = Subject.Create(invalid);
        subj.IsFailure.Should().BeTrue();
        subj.Error.Code.Should().Be("Subject.Required");
    }
}




