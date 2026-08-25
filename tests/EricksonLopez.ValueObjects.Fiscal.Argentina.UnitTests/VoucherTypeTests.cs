// Copyright © Erickson Lopez. MIT License.
using System;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Argentina;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Argentina.UnitTests;

public sealed class VoucherTypeTests
{
    [Fact]
    public void KnownInstances_DefaultState_HaveExactProperties()
    {
        // 1
        VoucherType.InvoiceA.Code.Should().Be(1);
        VoucherType.InvoiceA.Description.Should().Be("Factura A");
        VoucherType.InvoiceA.Letter.Should().Be('A');

        // 2
        VoucherType.DebitNoteA.Code.Should().Be(2);
        VoucherType.DebitNoteA.Description.Should().Be("Nota de Débito A");
        VoucherType.DebitNoteA.Letter.Should().Be('A');

        // 3
        VoucherType.CreditNoteA.Code.Should().Be(3);
        VoucherType.CreditNoteA.Description.Should().Be("Nota de Crédito A");
        VoucherType.CreditNoteA.Letter.Should().Be('A');

        // 6
        VoucherType.InvoiceB.Code.Should().Be(6);
        VoucherType.InvoiceB.Description.Should().Be("Factura B");
        VoucherType.InvoiceB.Letter.Should().Be('B');

        // 7
        VoucherType.DebitNoteB.Code.Should().Be(7);
        VoucherType.DebitNoteB.Description.Should().Be("Nota de Débito B");
        VoucherType.DebitNoteB.Letter.Should().Be('B');

        // 8
        VoucherType.CreditNoteB.Code.Should().Be(8);
        VoucherType.CreditNoteB.Description.Should().Be("Nota de Crédito B");
        VoucherType.CreditNoteB.Letter.Should().Be('B');

        // 11
        VoucherType.InvoiceC.Code.Should().Be(11);
        VoucherType.InvoiceC.Description.Should().Be("Factura C");
        VoucherType.InvoiceC.Letter.Should().Be('C');

        // 12
        VoucherType.DebitNoteC.Code.Should().Be(12);
        VoucherType.DebitNoteC.Description.Should().Be("Nota de Débito C");
        VoucherType.DebitNoteC.Letter.Should().Be('C');

        // 13
        VoucherType.CreditNoteC.Code.Should().Be(13);
        VoucherType.CreditNoteC.Description.Should().Be("Nota de Crédito C");
        VoucherType.CreditNoteC.Letter.Should().Be('C');

        // 19
        VoucherType.InvoiceE.Code.Should().Be(19);
        VoucherType.InvoiceE.Description.Should().Be("Factura de Exportación E");
        VoucherType.InvoiceE.Letter.Should().Be('E');

        // 201
        VoucherType.FceInvoiceA.Code.Should().Be(201);
        VoucherType.FceInvoiceA.Description.Should().Be("Factura de Crédito Electrónica MiPyME A");
        VoucherType.FceInvoiceA.Letter.Should().Be('A');

        // 206
        VoucherType.FceInvoiceB.Code.Should().Be(206);
        VoucherType.FceInvoiceB.Description.Should().Be("Factura de Crédito Electrónica MiPyME B");
        VoucherType.FceInvoiceB.Letter.Should().Be('B');

        // 211
        VoucherType.FceInvoiceC.Code.Should().Be(211);
        VoucherType.FceInvoiceC.Description.Should().Be("Factura de Crédito Electrónica MiPyME C");
        VoucherType.FceInvoiceC.Letter.Should().Be('C');

        VoucherType.InvoiceA.ToString().Should().Be("1 - Factura A");
    }

    [Theory]
    [InlineData(1, "Factura A")]
    [InlineData(2, "Nota de Débito A")]
    [InlineData(3, "Nota de Crédito A")]
    [InlineData(6, "Factura B")]
    [InlineData(7, "Nota de Débito B")]
    [InlineData(8, "Nota de Crédito B")]
    [InlineData(11, "Factura C")]
    [InlineData(12, "Nota de Débito C")]
    [InlineData(13, "Nota de Crédito C")]
    [InlineData(19, "Factura de Exportación E")]
    [InlineData(201, "Factura de Crédito Electrónica MiPyME A")]
    [InlineData(206, "Factura de Crédito Electrónica MiPyME B")]
    [InlineData(211, "Factura de Crédito Electrónica MiPyME C")]
    public void Create_All13OfficialCodes_Succeed(int code, string expectedDescription)
    {
        var result = VoucherType.Create(code);

        result.IsSuccess.Should().BeTrue();
        result.Value.Description.Should().Be(expectedDescription);
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("201", 201)]
    public void Create_NumericString_Succeeds(string input, int expectedCode)
    {
        var result = VoucherType.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(expectedCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(999)]
    public void Create_InvalidCode_ReturnsInvalidCodeError(int invalidCode)
    {
        var result = VoucherType.Create(invalidCode);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherType.InvalidCode");
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidFormat_ReturnsInvalidFormatError(string invalidFormat)
    {
        var result = VoucherType.Create(invalidFormat);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("VoucherType.InvalidFormat");
    }

    [Fact]
    public void VoucherType_DefaultState_ParseAndTryParse()
    {
        var parsed1 = VoucherType.Parse("1", System.Globalization.CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be(1);

        var parsed2 = VoucherType.Parse("1".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be(1);

        VoucherType.TryParse("1", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be(1);

        VoucherType.TryParse("1".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be(1);

        Action invalidParseStr = () => VoucherType.Parse("999", System.Globalization.CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid VoucherType: '999'.");

        Action invalidParseSpan = () => VoucherType.Parse("999".AsSpan(), System.Globalization.CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid VoucherType: '999'.");

        VoucherType.TryParse("999", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(VoucherType));

        VoucherType.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(VoucherType));

        VoucherType.TryParse("999".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(VoucherType));

        VoucherType.Create((string?)null).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void VoucherType_ComparisonsAndOperators_Exhaustive()
    {
        var a = VoucherType.InvoiceA; // 1
        var aCopy = VoucherType.Create(1).Value;
        var b = VoucherType.InvoiceB; // 6

        a.ShouldSatisfyEqualityContract(aCopy, b, (x, y) => x == y, (x, y) => x != y);
        a.ShouldSatisfyComparisonContract(aCopy, b,
            (x, y) => x < y,
            (x, y) => x <= y,
            (x, y) => x > y,
            (x, y) => x >= y);
    }
}




