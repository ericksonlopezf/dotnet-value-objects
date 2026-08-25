// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="DeletedBy"/> Domain Primitive.
/// </summary>
public sealed class DeletedByTests
{
    [Theory]
    [InlineData("ADMIN", "ADMIN")]
    [InlineData("  user_123  ", "USER_123")]
    [InlineData("system.bot/cron", "SYSTEM.BOT/CRON")]
    public void DeletedBy_WhenValid_ShouldSucceedAndNormalize(string input, string expected)
    {
        var deleted = DeletedBy.Create(input);
        deleted.IsSuccess.Should().BeTrue();
        deleted.Value.Value.Should().Be(expected);
        deleted.Value.ToString().Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DeletedBy_WhenBlank_ShouldFail(string? blank)
    {
        var deleted = DeletedBy.Create(blank);
        deleted.IsFailure.Should().BeTrue();
        deleted.Error.Code.Should().Be("DeletedBy.Required");
    }
}



