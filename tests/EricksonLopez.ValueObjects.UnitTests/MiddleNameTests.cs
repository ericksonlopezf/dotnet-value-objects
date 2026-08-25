// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="MiddleName"/> Value Object.
/// </summary>
public sealed class MiddleNameTests
{
    [Fact]
    public void MiddleName_CreateOptional_HandlesNullCleanly()
    {
        var nullResult = MiddleName.CreateOptional(null);
        var emptyResult = MiddleName.CreateOptional("   ");
        var validResult = MiddleName.CreateOptional(" Alexander ");

        nullResult.IsSuccess.Should().BeTrue();
        nullResult.Value.Should().BeNull();

        emptyResult.IsSuccess.Should().BeTrue();
        emptyResult.Value.Should().BeNull();

        validResult.IsSuccess.Should().BeTrue();
        validResult.Value!.Value.Should().Be("Alexander");
        validResult.Value.ToString().Should().Be("Alexander");
    }

    [Fact]
    public void MiddleName_InvalidFormat_ShouldFail()
    {
        var invalid = MiddleName.Create("Alex123");
        invalid.IsFailure.Should().BeTrue();
        invalid.Error.Code.Should().Be("MiddleName.InvalidFormat");
    }

    [Fact]
    public void MiddleName_ControlCharacters_ShouldFail()
    {
        var invalid = MiddleName.Create("Alex\0Middle");
        invalid.IsFailure.Should().BeTrue();
        invalid.Error.Code.Should().Be("MiddleName.ControlCharacters");
    }
}



