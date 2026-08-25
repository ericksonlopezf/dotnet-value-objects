// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ContentType"/> Value Object.
/// </summary>
public sealed class ContentTypeTests
{
    [Fact]
    public void ContentType_ValidMime_SucceedsAndNormalizesLower()
    {
        var pdf = ContentType.Create("APPLICATION/PDF").Value;
        var json = ContentType.Create("APPLICATION/JSON").Value;

        pdf.Value.Should().Be("application/pdf");
        json.Value.Should().Be("application/json");
        pdf.ToString().Should().Be("application/pdf");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid_mime")] // no slash
    [InlineData("text/ html")] // space in type
    public void ContentType_Invalid_ShouldFail(string? invalid)
    {
        var result = ContentType.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("ContentType.Required");
        else result.Error.Code.Should().Be("ContentType.InvalidFormat");
    }

    [Fact]
    public void ContentType_Invalid_ShouldReturnSpecificErrors()
    {
        var invalidPattern = ContentType.Create("invalid_mime");
        invalidPattern.Error.Description.Should().Be("Content type must use the media-type/subtype format.");

        var tooLong = ContentType.Create(new string('a', 256));
        tooLong.Error.Code.Should().Be("ContentType.TooLong");
    }
}




