// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="MessageBody"/> Domain Primitive.
/// </summary>
public sealed class MessageBodyTests
{
    [Fact]
    public void MessageBody_MultilineAndWhitespace_Succeeds()
    {
        var multiline = "Line 1\r\nLine 2\tTabbed";
        var msg = MessageBody.Create(multiline).Value;
        msg.Value.Should().Be(multiline);
        msg.ToString().Should().Be(multiline);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void MessageBody_Required_Fails(string? blank)
    {
        var res = MessageBody.Create(blank);
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("MessageBody.Required");
    }

    [Fact]
    public void MessageBody_ControlCharacters_Fails()
    {
        var res = MessageBody.Create("Bad\0Control");
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("MessageBody.ControlCharacters");
    }

    [Fact]
    public void MessageBody_TooLong_Fails()
    {
        var maxValid = MessageBody.Create(new string('a', 20000));
        maxValid.IsSuccess.Should().BeTrue();

        var res = MessageBody.Create(new string('a', 20001));
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("MessageBody.TooLong");
    }

    [Fact]
    public void MessageBody_EqualityContract()
    {
        var m1 = MessageBody.Create("Hello world").Value;
        var m1Copy = MessageBody.Create("Hello world").Value;
        var m2 = MessageBody.Create("Different content").Value;

        m1.ShouldSatisfyEqualityContract(m1Copy, m2, (a, b) => a == b, (a, b) => a != b);
    }
}



