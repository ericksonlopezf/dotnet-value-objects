// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Fiscal.Mexico;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico.UnitTests;

public sealed class PaymentFormCodeTests
{
    [Theory]
    [InlineData("01", "Efectivo", false)]
    [InlineData("02", "Cheque nominativo", false)]
    [InlineData("03", "Transferencia electrónica de fondos", false)]
    [InlineData("04", "Tarjeta de crédito", false)]
    [InlineData("28", "Tarjeta de débito", false)]
    [InlineData("99", "Por definir", true)]
    [InlineData("  01  ", "Efectivo", false)]
    public void Create_ValidOfficialCodes_ExtractsAllProperties(string input, string expectedDescription, bool isDeferred)
    {
        var result = PaymentFormCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input.Trim());
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.IsDeferred.Should().Be(isDeferred);
        result.Value.ToString().Should().Be($"{input.Trim()} - {expectedDescription}");
    }

    [Fact]
    public void StaticFields_DefaultState_ExhaustiveVerification()
    {
        PaymentFormCode.Cash.Code.Should().Be("01");
        PaymentFormCode.Check.Code.Should().Be("02");
        PaymentFormCode.WireTransfer.Code.Should().Be("03");
        PaymentFormCode.CreditCard.Code.Should().Be("04");
        PaymentFormCode.DebitCard.Code.Should().Be("28");
        PaymentFormCode.ToBeDefined.Code.Should().Be("99");
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("1")]
    [InlineData("100")]
    [InlineData("A1")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_InvalidFormat_ReturnsError(string? input)
    {
        var result = PaymentFormCode.Create(input);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().StartWith("PaymentFormCode.Invalid");
    }

    [Theory]
    [InlineData("00")]
    [InlineData("05")]
    [InlineData("98")]
    public void Create_UnknownButValidFormatCode_ReturnsDynamicCatalog(string input)
    {
        var result = PaymentFormCode.Create(input);

        result.IsSuccess.Should().BeTrue();
        result.Value.Code.Should().Be(input);
        result.Value.Description.Should().Be("Forma de Pago (Catálogo Dinámico)");
        result.Value.IsDeferred.Should().BeFalse();
    }

    [Fact]
    public void PaymentFormCode_DefaultState_ComparisonOperators()
    {
        var p1 = PaymentFormCode.Cash;
        var p2 = PaymentFormCode.ToBeDefined;
        var p1Clone = PaymentFormCode.Create("01").Value;

        (p1 < p2).Should().BeTrue();
        (p1 <= p2).Should().BeTrue();
        (p2 > p1).Should().BeTrue();
        (p2 >= p1).Should().BeTrue();

        (p1 < p1Clone).Should().BeFalse();
        (p1 > p1Clone).Should().BeFalse();
        (p1 <= p1Clone).Should().BeTrue();
        (p1 >= p1Clone).Should().BeTrue();
        p1.CompareTo(p2).Should().BeNegative();
        p2.CompareTo(p1).Should().BePositive();
        p1.CompareTo(p1Clone).Should().Be(0);
    }

    [Fact]
    public void PaymentFormCode_DefaultState_ParseAndTryParse()
    {
        var parsed1 = PaymentFormCode.Parse("01", CultureInfo.InvariantCulture);
        parsed1.Code.Should().Be("01");

        var parsed2 = PaymentFormCode.Parse("01".AsSpan(), CultureInfo.InvariantCulture);
        parsed2.Code.Should().Be("01");

        PaymentFormCode.TryParse("01", null, out var tryRes1).Should().BeTrue();
        tryRes1.Code.Should().Be("01");

        PaymentFormCode.TryParse("01".AsSpan(), null, out var tryRes2).Should().BeTrue();
        tryRes2.Code.Should().Be("01");

        Action invalidParseStr = () => PaymentFormCode.Parse("invalid", CultureInfo.InvariantCulture);
        invalidParseStr.Should().Throw<FormatException>().WithMessage("Invalid PaymentFormCode: 'invalid'.");

        Action invalidParseSpan = () => PaymentFormCode.Parse("invalid".AsSpan(), CultureInfo.InvariantCulture);
        invalidParseSpan.Should().Throw<FormatException>().WithMessage("Invalid PaymentFormCode: 'invalid'.");

        PaymentFormCode.TryParse("invalid", null, out var tryFail1).Should().BeFalse();
        tryFail1.Should().Be(default(PaymentFormCode));

        PaymentFormCode.TryParse((string?)null, null, out var tryFailNull).Should().BeFalse();
        tryFailNull.Should().Be(default(PaymentFormCode));

        PaymentFormCode.TryParse("invalid".AsSpan(), null, out var tryFail2).Should().BeFalse();
        tryFail2.Should().Be(default(PaymentFormCode));
    }
}




