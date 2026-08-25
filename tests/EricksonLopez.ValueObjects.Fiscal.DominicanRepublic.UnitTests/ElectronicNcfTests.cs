// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class ElectronicNcfTests
{
    [Fact]
    public void Create_ValidString_ExtractsTypeAndSequence()
    {
        var result = ElectronicNcf.Create("E310000000001");

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("E310000000001");
        result.Value.Type.Should().Be(EcfType.ElectronicCreditFiscal);
        result.Value.Sequence.Should().Be(1);
        ElectronicNcf.Series.Should().Be('E');
    }

    [Fact]
    public void Create_FromTypeAndSequence_FormatsCorrectly()
    {
        var result = ElectronicNcf.Create(EcfType.ElectronicConsumer, 123456L);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("E320000123456");
        result.Value.Sequence.Should().Be(123456L);
    }

    [Fact]
    public void WithSecurityCode_DefaultState_AssociatesDgiiSecurityCode()
    {
        var ecf = ElectronicNcf.Create("E310000000001").Value;
        var secCode = SecurityCode.Create("AB12CD").Value;

        var ecfWithSec = ecf.WithSecurityCode(secCode);

        ecfWithSec.SecurityCode.Should().NotBeNull();
        ecfWithSec.SecurityCode!.Value.Should().Be("AB12CD");

        var ecfDirectSec = ElectronicNcf.Create(EcfType.ElectronicCreditFiscal, 100L, secCode).Value;
        ecfDirectSec.SecurityCode.Should().Be(secCode);

        var ecfFromStringWithSec = ElectronicNcf.Create("E310000000001", secCode).Value;
        ecfFromStringWithSec.SecurityCode.Should().Be(secCode);

        Action nullSec = () => ecf.WithSecurityCode(null!);
        nullSec.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrWhitespace_ReturnsRequiredError(string? invalid)
    {
        var result = ElectronicNcf.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ElectronicNcf.Required");
    }

    [Theory]
    [InlineData("B310000000001")] // Wrong series (B instead of E)
    [InlineData("E31000000001")]  // 12 chars (too short)
    [InlineData("E3100000000001")]// 14 chars (too long)
    [InlineData("E990000000001")] // Invalid type 99
    [InlineData("E31000000000A")] // Non-digit sequence
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalid)
    {
        var result = ElectronicNcf.Create(invalid);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ElectronicNcf.InvalidFormat");
    }

    [Fact]
    public void Create_SequenceZero_ReturnsInvalidSequenceError()
    {
        var result = ElectronicNcf.Create("E310000000000");
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ElectronicNcf.InvalidSequence");
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(-1L)]
    [InlineData(10_000_000_000L)]
    public void Create_FromTypeAndInvalidSequence_ReturnsSequenceOutOfRange(long invalidSeq)
    {
        var result = ElectronicNcf.Create(EcfType.ElectronicCreditFiscal, invalidSeq);
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("ElectronicNcf.SequenceOutOfRange");
    }

    [Fact]
    public void Create_MaxSequence_Succeeds()
    {
        var result = ElectronicNcf.Create(EcfType.ElectronicCreditFiscal, 9_999_999_999L);
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be("E319999999999");
        result.Value.Sequence.Should().Be(9_999_999_999L);
    }

    [Fact]
    public void ElectronicNcf_Equality_AndToString()
    {
        var a = ElectronicNcf.Create("E310000000001").Value;
        var b = ElectronicNcf.Create(EcfType.ElectronicCreditFiscal, 1).Value;
        var c = ElectronicNcf.Create("E320000000001").Value;

        (a == b).Should().BeTrue();
        (a != c).Should().BeTrue();
        a.ToString().Should().Be("E310000000001");
    }
}




