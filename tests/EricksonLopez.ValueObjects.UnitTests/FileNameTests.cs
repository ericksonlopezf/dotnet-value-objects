// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Unit tests for the <see cref="FileName"/> Value Object.
/// </summary>
public sealed class FileNameTests
{
    [Fact]
    public void FileName_Valid_ExtractsExtension()
    {
        var file = FileName.Create("invoice_2026.pdf").Value;
        file.Extension.Should().Be(".pdf");
        file.ToString().Should().Be("invoice_2026.pdf");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("report:final.pdf")]
    [InlineData("file?.docx")]
    [InlineData("invalid/path/file.txt")]
    [InlineData("file\\test.txt")]
    [InlineData("file*name.txt")]
    [InlineData("file<name>.txt")]
    [InlineData("file|pipe.txt")]
    [InlineData("file\"quote.txt")]
    public void FileName_InvalidCharacters_ShouldFail(string? invalid)
    {
        var result = FileName.Create(invalid);
        result.IsFailure.Should().BeTrue();
        if (string.IsNullOrWhiteSpace(invalid)) result.Error.Code.Should().Be("FileName.Required");
        else result.Error.Code.Should().Be("FileName.InvalidCharacters");
    }

    [Fact]
    public void FileName_Invalid_ShouldReturnSpecificErrors()
    {
        var invalidChar = FileName.Create("file?.docx");
        invalidChar.Error.Description.Should().Be("File name contains characters that are invalid on Windows, Linux, or cloud storage platforms.");

        var tooLong = FileName.Create(new string('a', 256));
        tooLong.Error.Code.Should().Be("FileName.TooLong");

        FileName.Create("document.pdf").Value.Extension.Should().Be(".pdf");
        FileName.Create("archive.tar.gz").Value.Extension.Should().Be(".gz");
        FileName.Create(".gitignore").Value.Extension.Should().Be(".gitignore");
        FileName.Create("Makefile").Value.Extension.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("file\x00name.txt")]
    [InlineData("file\x1Fname.txt")]
    public void FileName_ControlCharacters_ShouldFail(string controlCharName)
    {
        var result = FileName.Create(controlCharName);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("FileName.ControlCharacters");
    }

    [Fact]
    public void FileName_EqualityContract()
    {
        var f1 = FileName.Create("test.txt").Value;
        var f1Copy = FileName.Create("test.txt").Value;
        var f2 = FileName.Create("other.txt").Value;

        f1.ShouldSatisfyEqualityContract(f1Copy, f2, (a, b) => a == b, (a, b) => a != b);
    }
}




