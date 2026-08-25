// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Chile;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Chile.UnitTests;

public sealed class DocumentReferenceTests
{
    [Theory]
    [InlineData((byte)1, "Anula factura por error de emisión")]
    [InlineData((byte)2, "Corrige razón social")]
    [InlineData((byte)3, "Corrige montos facturados")]
    [InlineData((byte)1, null)]
    public void Create_ValidParameters_ExtractsAllProperties(byte referenceCode, string? reason)
    {
        var targetType = DteTypeCode.FacturaElectronica;
        var folio = FiscalFolio.Create(500).Value;
        var date = new DateOnly(2026, 8, 1);

        var result = DocumentReference.Create(targetType, folio, date, referenceCode, reason);

        result.IsSuccess.Should().BeTrue();
        result.Value.TargetType.Should().Be(targetType);
        result.Value.Folio.Should().Be(folio);
        result.Value.Date.Should().Be(date);
        result.Value.ReferenceCode.Should().Be(referenceCode);
        result.Value.Reason.Should().Be(reason);
        result.Value.ToString().Should().Be($"DTE 33 Folio 500 (2026-08-01) CodRef: {referenceCode.ToString(CultureInfo.InvariantCulture)}");
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)4)]
    [InlineData((byte)255)]
    public void Create_InvalidReferenceCode_ReturnsError(byte referenceCode)
    {
        var targetType = DteTypeCode.FacturaElectronica;
        var folio = FiscalFolio.Create(500).Value;
        var date = new DateOnly(2026, 8, 1);

        var result = DocumentReference.Create(targetType, folio, date, referenceCode);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("DocumentReference.InvalidReferenceCode");
    }

    [Fact]
    public void DocumentReference_DefaultState_Equality()
    {
        var targetType = DteTypeCode.FacturaElectronica;
        var folio = FiscalFolio.Create(500).Value;
        var date = new DateOnly(2026, 8, 1);

        var doc1 = DocumentReference.Create(targetType, folio, date, 1, "Reason").Value;
        var doc2 = DocumentReference.Create(targetType, folio, date, 1, "Reason").Value;
        var docDiff = DocumentReference.Create(targetType, folio, date, 2, "Reason").Value;

        (doc1 == doc2).Should().BeTrue();
        (doc1 != docDiff).Should().BeTrue();
        doc1.Equals(doc2).Should().BeTrue();
        doc1.Equals((object)doc2).Should().BeTrue();
        doc1.Equals(docDiff).Should().BeFalse();
        doc1.GetHashCode().Should().Be(doc2.GetHashCode());
    }
}




