// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="ExternalReference"/> Domain Primitive.
/// </summary>
public sealed class ExternalReferenceTests
{
    [Fact]
    public void ExternalReference_DefaultState_PreservesCaseExact()
    {
        var external = ExternalReference.Create("  Stripe_ch_3M4oLa2eZvKYlo2C19XyZzWq  ").Value;
        external.Value.Should().Be("Stripe_ch_3M4oLa2eZvKYlo2C19XyZzWq");
        external.ToString().Should().Be("Stripe_ch_3M4oLa2eZvKYlo2C19XyZzWq");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("External\0Ref")] // control char
    public void ExternalReference_Invalid_ShouldFail(string? invalid)
    {
        var result = ExternalReference.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("ExternalReference.Required");
        else result.Error.Code.Should().Be("ExternalReference.ControlCharacters");
    }

    [Fact]
    public void ExternalReference_Invalid_ShouldReturnSpecificErrors()
    {
        ExternalReference.Create(new string('A', 201)).Error.Code.Should().Be("ExternalReference.TooLong");
    }
}




