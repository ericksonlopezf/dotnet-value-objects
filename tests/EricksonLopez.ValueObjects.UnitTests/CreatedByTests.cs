// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="CreatedBy"/> Domain Primitive.
/// </summary>
public sealed class CreatedByTests
{
    [Theory]
    [InlineData("ADMIN", "ADMIN")]
    [InlineData("  user_123  ", "USER_123")]
    [InlineData("system.bot/cron", "SYSTEM.BOT/CRON")]
    public void CreatedBy_WhenValid_ShouldSucceedAndNormalize(string input, string expected)
    {
        var created = CreatedBy.Create(input);
        created.IsSuccess.Should().BeTrue();
        created.Value.Value.Should().Be(expected);
        created.Value.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreatedBy_WhenBlank_ShouldFail(string? blank)
    {
        var created = CreatedBy.Create(blank);
        created.IsFailure.Should().BeTrue();
        created.Error.Code.Should().Be("CreatedBy.Required");
    }
}



