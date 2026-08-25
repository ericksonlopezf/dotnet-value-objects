// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class StringPipelineTests
{
    [Fact]
    public void CollapseWhitespace_NullOrEmpty_ReturnsEmpty()
    {
        StringPipeline.CollapseWhitespace(null!).Should().BeEmpty();
        StringPipeline.CollapseWhitespace("").Should().BeEmpty();
        StringPipeline.CollapseWhitespace("   ").Should().BeEmpty();
    }

    [Fact]
    public void CollapseWhitespace_SingleWord_PreservesWithoutLeadingOrTrailingSpaces()
    {
        StringPipeline.CollapseWhitespace("hello").Should().Be("hello");
        StringPipeline.CollapseWhitespace("  hello").Should().Be("hello");
        StringPipeline.CollapseWhitespace("hello  ").Should().Be("hello");
        StringPipeline.CollapseWhitespace("  hello  ").Should().Be("hello");
    }

    [Fact]
    public void CollapseWhitespace_MultipleSpacesAndTabs_CollapsesToSingleSpace()
    {
        var input = "  Hello   \t  World   from   StringPipeline  ";
        var result = StringPipeline.CollapseWhitespace(input);

        result.Should().Be("Hello World from StringPipeline");
    }

    [Fact]
    public void NormalizeHumanName_And_NormalizeBusinessName_CollapseWhitespace()
    {
        StringPipeline.NormalizeHumanName("  John   Doe  ").Should().Be("John Doe");
        StringPipeline.NormalizeBusinessName("  Acme   Corp  ").Should().Be("Acme Corp");
    }

    [Fact]
    public void NormalizeTrimUpper_And_NormalizeLower_TransformCorrectly()
    {
        StringPipeline.NormalizeTrimUpper("  abc  ").Should().Be("ABC");
        StringPipeline.NormalizeLower("  XYZ  ").Should().Be("xyz");
        StringPipeline.NormalizeCode("  code   123  ").Should().Be("CODE 123");
    }

    [Fact]
    public void ContainsControlCharacters_DefaultState_DetectsControls()
    {
        StringPipeline.ContainsControlCharacters("Valid Text").Should().BeFalse();
        StringPipeline.ContainsControlCharacters("Invalid\0Text").Should().BeTrue();
        StringPipeline.ContainsControlCharacters("Invalid\u0007Text").Should().BeTrue();
    }

    [Fact]
    public void Required_NullOrWhitespace_FailsWithRequiredError()
    {
        var result = StringPipeline.Required<string>(
            "   ", "TestField", 1, 10, static s => s);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TestField.Required");
        result.Error.Description.Should().Be("TestField is required.");
    }

    [Fact]
    public void Required_ContainsControlCharacters_FailsWithControlError()
    {
        var result = StringPipeline.Required<string>(
            "Bad\0Data", "TestField", 1, 10, static s => s);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TestField.ControlCharacters");
        result.Error.Description.Should().Be("TestField cannot contain control characters.");
    }

    [Fact]
    public void Required_TooShort_FailsWithTooShortError()
    {
        var result = StringPipeline.Required<string>(
            "ab", "TestField", 5, 10, static s => s);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TestField.TooShort");
        result.Error.Description.Should().Be("TestField must contain at least 5 characters.");
    }

    [Fact]
    public void Required_TooLong_FailsWithTooLongError()
    {
        var result = StringPipeline.Required<string>(
            "abcdefghijk", "TestField", 1, 5, static s => s);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("TestField.TooLong");
        result.Error.Description.Should().Be("TestField must contain at most 5 characters.");
    }

    [Fact]
    public void Required_PatternMismatch_FailsWithCustomMessageOrFallback()
    {
        var pattern = new Regex(@"^\d+$");
        var customRes = StringPipeline.Required<string>(
            "abc", "DigitsOnly", 1, 10, static s => s, pattern: pattern, patternMessage: "Must be digits.");

        customRes.IsFailure.Should().BeTrue();
        customRes.Error.Code.Should().Be("DigitsOnly.InvalidFormat");
        customRes.Error.Description.Should().Be("Must be digits.");

        var defaultMsgRes = StringPipeline.Required<string>(
            "abc", "DigitsOnly", 1, 10, static s => s, pattern: pattern, patternMessage: null);

        defaultMsgRes.IsFailure.Should().BeTrue();
        defaultMsgRes.Error.Code.Should().Be("DigitsOnly.InvalidFormat");
        defaultMsgRes.Error.Description.Should().Be("DigitsOnly has an invalid format.");
    }

    [Fact]
    public void Required_Valid_AppliesNormalizeAndCallsFactory()
    {
        var result = StringPipeline.Required<string>(
            "  valid  ", "Field", 1, 10, static s => s, normalize: static s => s.Trim().ToUpperInvariant());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("VALID");
    }

    [Fact]
    public void Required_WithoutNormalize_UsesTrimByDefault()
    {
        var result = StringPipeline.Required<string>(
            "  plain  ", "Field", 1, 10, static s => s, normalize: null);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("plain");
    }

    [Fact]
    public void RequiredString_DefaultState_DelegatesCorrectly()
    {
        var result = StringPipeline.RequiredString(
            "  string  ", "Field", 1, 10);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("string");
    }

    [Fact]
    public void RegexPatterns_ShouldValidateExpectedFormats()
    {
        // HumanNamePattern
        StringPipeline.HumanNamePattern.Options.Should().HaveFlag(RegexOptions.CultureInvariant);
        StringPipeline.HumanNamePattern.IsMatch("Jean-Luc D'Arcy O. Smith").Should().BeTrue();
        StringPipeline.HumanNamePattern.IsMatch("John123").Should().BeFalse();
        StringPipeline.HumanNamePattern.IsMatch("").Should().BeFalse();

        // BusinessNamePattern
        StringPipeline.BusinessNamePattern.Options.Should().HaveFlag(RegexOptions.CultureInvariant);
        StringPipeline.BusinessNamePattern.IsMatch("Acme & Co. (Global) #1/A @Main +Co.").Should().BeTrue();
        StringPipeline.BusinessNamePattern.IsMatch("Acme <Corp>").Should().BeFalse();
        StringPipeline.BusinessNamePattern.IsMatch("").Should().BeFalse();

        // CodePattern
        StringPipeline.CodePattern.Options.Should().HaveFlag(RegexOptions.CultureInvariant);
        StringPipeline.CodePattern.IsMatch("CODE_123.A-B/C").Should().BeTrue();
        StringPipeline.CodePattern.IsMatch("_INVALID").Should().BeFalse();
        StringPipeline.CodePattern.IsMatch("CODE WITH SPACE").Should().BeFalse();
        StringPipeline.CodePattern.IsMatch("").Should().BeFalse();

        // LooseIdentifierPattern
        StringPipeline.LooseIdentifierPattern.Options.Should().HaveFlag(RegexOptions.CultureInvariant);
        StringPipeline.LooseIdentifierPattern.IsMatch("CODE 123.A-B/C").Should().BeTrue();
        StringPipeline.LooseIdentifierPattern.IsMatch(" INVALID").Should().BeFalse();
        StringPipeline.LooseIdentifierPattern.IsMatch("").Should().BeFalse();
    }
}




