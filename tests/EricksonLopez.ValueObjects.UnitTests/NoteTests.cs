// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="Note"/> Domain Primitive.
/// </summary>
public sealed class NoteTests
{
    [Fact]
    public void Note_MultilineAndWhitespace_Succeeds()
    {
        var multiline = "Line 1\r\nLine 2\tTabbed";
        var note = Note.Create(multiline).Value;
        note.Value.Should().Be(multiline);
        note.ToString().Should().Be(multiline);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Note_Required_Fails(string? blank)
    {
        var res = Note.Create(blank);
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Note.Required");
    }

    [Fact]
    public void Note_ControlCharacters_Fails()
    {
        var res = Note.Create("Bad\0Control");
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Note.ControlCharacters");
    }

    [Fact]
    public void Note_TooLong_Fails()
    {
        var maxValid = Note.Create(new string('a', 5000));
        maxValid.IsSuccess.Should().BeTrue();

        var res = Note.Create(new string('a', 5001));
        res.IsFailure.Should().BeTrue();
        res.Error.Code.Should().Be("Note.TooLong");
    }

    [Fact]
    public void Note_EqualityContract()
    {
        var n1 = Note.Create("Important note").Value;
        var n1Copy = Note.Create("Important note").Value;
        var n2 = Note.Create("Other note").Value;

        n1.ShouldSatisfyEqualityContract(n1Copy, n2, (a, b) => a == b, (a, b) => a != b);
    }
}



