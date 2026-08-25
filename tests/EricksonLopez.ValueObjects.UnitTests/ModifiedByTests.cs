// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ModifiedBy"/> Domain Primitive.
/// </summary>
public sealed class ModifiedByTests
{
    [Theory]
    [InlineData("ADMIN", "ADMIN")]
    [InlineData("  user_123  ", "USER_123")]
    [InlineData("system.bot/cron", "SYSTEM.BOT/CRON")]
    public void ModifiedBy_WhenValid_ShouldSucceedAndNormalize(string input, string expected)
    {
        var modified = ModifiedBy.Create(input);
        modified.IsSuccess.Should().BeTrue();
        modified.Value.Value.Should().Be(expected);
        modified.Value.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ModifiedBy_WhenBlank_ShouldFail(string? blank)
    {
        var modified = ModifiedBy.Create(blank);
        modified.IsFailure.Should().BeTrue();
        modified.Error.Code.Should().Be("ModifiedBy.Required");
    }
}



