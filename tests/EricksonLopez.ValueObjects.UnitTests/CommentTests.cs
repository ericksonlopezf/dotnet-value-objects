// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Comment"/> Domain Primitive.
/// </summary>
public sealed class CommentTests
{
    [Fact]
    public void Comment_MultilineAndWhitespace_Succeeds()
    {
        var multiline = "Line 1\r\nLine 2\tTabbed";
        var comment = Comment.Create(multiline).Value;
        comment.Value.Should().Be(multiline);
        comment.ToString().Should().Be(multiline);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Comment_Required_Fails(string? blank)
    {
        var res = Comment.Create(blank);
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Comment.Required");
    }

    [Fact]
    public void Comment_ControlCharacters_Fails()
    {
        var res = Comment.Create("Bad\0Control");
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Comment.ControlCharacters");
    }

    [Fact]
    public void Comment_TooLong_Fails()
    {
        var maxValid = Comment.Create(new string('a', 5000));
        maxValid.IsSuccess.Should().BeTrue();

        var res = Comment.Create(new string('a', 5001));
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Comment.TooLong");
    }

    [Fact]
    public void Comment_EqualityContract()
    {
        var c1 = Comment.Create("General comment").Value;
        var c1Copy = Comment.Create("General comment").Value;
        var c2 = Comment.Create("Other comment").Value;

        c1.ShouldSatisfyEqualityContract(c1Copy, c2, (a, b) => a == b, (a, b) => a != b);
    }
}



