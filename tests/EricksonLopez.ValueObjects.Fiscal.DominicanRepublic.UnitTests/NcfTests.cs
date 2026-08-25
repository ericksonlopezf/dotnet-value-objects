// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class NcfTests
{
    [Fact]
    public void Create_ValidString_ExtractsTypeAndSequence()
    {
        var result = Ncf.Create("B0100000001");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("B0100000001");
        result.Value.Type.Should().Be(NcfType.CreditFiscal);
        result.Value.Sequence.Should().Be(1);
        Ncf.Series.Should().Be('B');
    }

    [Fact]
    public void Create_FromTypeAndSequence_FormatsCorrectly()
    {
        var result = Ncf.Create(NcfType.Consumer, 42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("B0200000042");
        result.Value.Sequence.Should().Be(42);
    }

    [Fact]
    public void Create_InvalidType_ReturnsFailure()
    {
        var result = Ncf.Create("B9900000001");

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ncf.InvalidFormat");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = Ncf.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ncf.Required");
    }

    [Theory]
    [InlineData("A0100000001")] // Wrong series
    [InlineData("B010000001")]  // 10 chars
    [InlineData("B01000000001")]// 12 chars
    [InlineData("B010000000A")] // Non-digit sequence
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = Ncf.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ncf.InvalidFormat");
    }

    [Fact]
    public void Create_SequenceZero_ReturnsInvalidSequenceError()
    {
        var result = Ncf.Create("B0100000000");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ncf.InvalidSequence");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(100_000_000)]
    public void Create_FromTypeAndInvalidSequence_ReturnsSequenceOutOfRange(int invalidSeq)
    {
        var result = Ncf.Create(NcfType.CreditFiscal, invalidSeq);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Ncf.SequenceOutOfRange");
    }

    [Fact]
    public void Create_MaxSequence_Succeeds()
    {
        var result = Ncf.Create(NcfType.CreditFiscal, 99_999_999);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("B0199999999");
        result.Value.Sequence.Should().Be(99_999_999);
    }

    [Fact]
    public void Ncf_Equality_AndToString()
    {
        var a = Ncf.Create("B0100000001").Value;
        var b = Ncf.Create(NcfType.CreditFiscal, 1).Value;
        var c = Ncf.Create("B0200000001").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        a.ToString().Should().Be("B0100000001");
    }
}




