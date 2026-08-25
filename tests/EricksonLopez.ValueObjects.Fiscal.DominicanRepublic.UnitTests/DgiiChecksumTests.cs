// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic.UnitTests;

public sealed class DgiiChecksumTests
{
    [Theory]
    [InlineData("101000015", true)]   // Valid DGII test RNC (sum=17, rem=6, check=5)
    [InlineData("131880738", true)]   // Real registered RNC (sum=157, rem=3, check=8)
    [InlineData("100001002", true)]   // Remainder = 0 -> check digit = 2
    [InlineData("000000002", true)]   // Sum = 0 -> Remainder = 0 -> check digit = 2
    [InlineData("100000111", true)]   // Remainder = 1 -> check digit = 1
    [InlineData("100000209", true)]   // Remainder = 2 -> check digit = 9
    [InlineData("900000003", true)]   // First digit = '9' -> sum = 63, rem = 8 -> check digit = 3
    [InlineData("101000012", false)]  // Checksum mismatch
    [InlineData("10100001", false)]   // 8 digits (too short)
    [InlineData("1010000111", false)] // 10 digits (too long)
    [InlineData("10100001A", false)]  // Non-digit check character
    [InlineData("10100001:", false)]  // Character ':' ('9' + 1) at check digit
    [InlineData("10100001/", false)]  // Character '/' ('0' - 1) at check digit
    [InlineData("A01000015", false)]  // Non-digit first character
    [InlineData(":01000015", false)]  // Character ':' at first character
    [InlineData("/01000015", false)]  // Character '/' at first character
    [InlineData("1010A0015", false)]  // Non-digit intermediate character
    [InlineData("101000010", false)]  // Incorrect check digit '0'
    public void ValidateRnc_DefaultState_ShouldVerifyCheckDigit(string rnc, bool expected)


    {
        bool isValid = DgiiChecksum.ValidateRnc(rnc.AsSpan());
        isValid.Should().Be(expected);
    }
}



