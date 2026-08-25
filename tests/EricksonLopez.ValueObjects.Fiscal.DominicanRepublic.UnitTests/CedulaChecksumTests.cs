// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class CedulaChecksumTests
{
    [Theory]
    [InlineData("00112345673", true)]  // Valid calculated (sum=27, rem=7, check=3)
    [InlineData("40200000004", true)]  // Valid calculated (sum=6, rem=6, check=4)
    [InlineData("00100000900", true)]  // Sum=10 -> rem=0 -> (10-0)%10 = 0 -> check digit 0
    [InlineData("00000000109", true)]  // Sum=1 -> rem=1 -> (10-1)%10 = 9 -> check digit 9
    [InlineData("00100000003", false)] // Invalid check digit
    [InlineData("00100000001", false)] // Invalid check digit
    [InlineData("0011234567", false)]  // 10 digits (too short)
    [InlineData("001123456733", false)]// 12 digits (too long)
    [InlineData("0011234567A", false)] // Non-digit at check digit
    [InlineData("0011234567:", false)] // Character ':' ('9' + 1) at check digit
    [InlineData("0011234567/", false)] // Character '/' ('0' - 1) at check digit
    [InlineData("A0112345673", false)] // Non-digit at first position
    [InlineData(":0112345673", false)] // Character ':' at first position
    [InlineData("/0112345673", false)] // Character '/' at first position
    [InlineData("001123456A3", false)] // Non-digit at intermediate position
    public void ValidateCedula_DefaultState_CorrectlyEvaluates(string cedula, bool expected)

    {
        bool isValid = CedulaChecksum.ValidateCedula(cedula.AsSpan());
        isValid.Should().Be(expected);
    }
}



