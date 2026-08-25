// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class DomainExceptionTests
{
    [Fact]
    public void Constructor_WithMessage_ShouldSetMessageProperty()
    {
        var ex = new DomainException("Domain invariant violated");

        ex.Message.Should().Be("Domain invariant violated");
        ex.InnerException.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
    {
        var inner = new InvalidOperationException("Inner error");
        var ex = new DomainException("Outer domain error", inner);

        ex.Message.Should().Be("Outer domain error");
        ex.InnerException.Should().BeSameAs(inner);
    }

    [Fact]
    public void ThrowIf_ShouldThrow_WhenConditionIsTrue()
    {
        Action act = () => DomainException.ThrowIf(true, "Condition was true");

        act.Should().Throw<DomainException>()
            .WithMessage("Condition was true");
    }

    [Fact]
    public void ThrowIf_ShouldNotThrow_WhenConditionIsFalse()
    {
        Action act = () => DomainException.ThrowIf(false, "Condition was false");

        act.Should().NotThrow();
    }
}



