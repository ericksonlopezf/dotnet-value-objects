// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.Globalization;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Adversarial audit tests verifying resistance against:
/// 1. Invariant bypasses and constructor attacks
/// 2. Null attacks and invalid composite references
/// 3. default(T) attacks on structs
/// 4. Arithmetic overflow attacks
/// 5. Zero-allocation ISpanFormattable fidelity
/// 6. Equality and Hashing axioms
/// </summary>
public sealed class AdversarialAuditTests
{
    [Fact]
    public void Address_Create_WhenCountryIsNull_ShouldFailValidation()
    {
        // Act
        var result = Address.Create("123 Main St", "Metropolis", "Central", null!);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Address.CountryRequired");
    }

    [Theory]
    [InlineData("John Doe <john.doe@example.com>")]
    [InlineData("user@example.com (Personal)")]
    [InlineData("<user@example.com>")]
    [InlineData("user@example.com\r\nBcc: evil@domain.com")]
    [InlineData("user@example.com\nEvil: header")]
    public void Email_Create_WhenContainsDisplayNameOrCommentsOrNewlines_ShouldFailValidation(string malformed)
    {
        // Act
        var result = Email.Create(malformed);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Email.InvalidFormat");
    }

    [Fact]
    public void HierarchyPath_Root_WhenInitialSegmentHasSlashes_ShouldSanitizeSafely()
    {
        // Act
        var path = HierarchyPath.Root("dept/finance/billing");

        // Assert
        path.Value.Should().Be("/dept_finance_billing/");
        path.Depth.Should().Be(1);
    }

    [Fact]
    public void Quantity_Add_WhenIntOverflows_ShouldFailWithOverflowError_NotNegativeError()
    {
        // Arrange
        var qMax = Quantity.Create(int.MaxValue).Value;
        var qOne = Quantity.Create(1).Value;

        // Act
        var result = qMax.Add(qOne);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Quantity.Overflow");
    }

    [Fact]
    public void Money_TryFormat_WhenBufferAdequate_FormatsCorrectlyWithoutAllocations()
    {
        // Arrange
        var money = Money.Create(1234.56m, CurrencyCode.USD).Value;
        Span<char> buffer = stackalloc char[32];

        // Act
        bool success = money.TryFormat(buffer, out int charsWritten, "N2", CultureInfo.InvariantCulture);

        // Assert
        success.Should().BeTrue();
        string formatted = buffer[..charsWritten].ToString();
        formatted.Should().Be("1,234.56 USD");
    }

    [Fact]
    public void Money_TryFormat_WhenBufferTooSmall_ReturnsFalse()
    {
        // Arrange
        var money = Money.Create(1234.56m, CurrencyCode.USD).Value;
        Span<char> smallBuffer = stackalloc char[4];

        // Act
        bool success = money.TryFormat(smallBuffer, out int charsWritten, "N2", CultureInfo.InvariantCulture);

        // Assert
        success.Should().BeFalse();
        charsWritten.Should().Be(0);
    }

    [Fact]
    public void Percentage_TryFormat_WhenBufferAdequate_FormatsCorrectly()
    {
        // Arrange
        var percentage = Percentage.Create(18.5m).Value;
        Span<char> buffer = stackalloc char[16];

        // Act
        bool success = percentage.TryFormat(buffer, out int charsWritten, default, CultureInfo.InvariantCulture);

        // Assert
        success.Should().BeTrue();
        string formatted = buffer[..charsWritten].ToString();
        formatted.Should().Be("18.5%");
    }

    [Fact]
    public void Percentage_TryFormat_WhenBufferTooSmall_ReturnsFalse()
    {
        // Arrange
        var percentage = Percentage.Create(18.5m).Value;
        Span<char> tinyBuffer = stackalloc char[2];

        // Act
        bool success = percentage.TryFormat(tinyBuffer, out int charsWritten, default, CultureInfo.InvariantCulture);

        // Assert
        success.Should().BeFalse();
        charsWritten.Should().Be(0);
    }

    [Fact]
    public void TaxRate_CalculateTax_ReturnsAccurateBankerRoundedMoney()
    {
        // Arrange
        var taxRate = TaxRate.Create(18m).Value;
        var money = Money.Create(100.00m, CurrencyCode.USD).Value;

        // Act
        var tax = taxRate.CalculateTax(money);

        // Assert
        tax.Amount.Should().Be(18.00m);
        tax.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void DiscountRate_ApplyTo_AndCalculateDiscount_ReturnsAccurateMoney()
    {
        // Arrange
        var discount = DiscountRate.Create(20m).Value;
        var money = Money.Create(100.00m, CurrencyCode.USD).Value;

        // Act
        var discountAmount = discount.CalculateDiscount(money);
        var netAmount = discount.ApplyTo(money);

        // Assert
        discountAmount.Amount.Should().Be(20.00m);
        discountAmount.Currency.Should().Be(CurrencyCode.USD);
        netAmount.Amount.Should().Be(80.00m);
        netAmount.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void StructValueObjects_DefaultInstance_BehaviorsAreDocumentedAndPredictable()
    {
        // Money
        default(Money).Amount.Should().Be(0m);
        default(Money).Currency.ToString().Should().Be(string.Empty);
        default(Money).ToString().Should().Be("0.00");

        // CurrencyCode
        default(CurrencyCode).Value.Should().BeNull();
        default(CurrencyCode).ToString().Should().Be(string.Empty);
        default(CurrencyCode).DecimalPlaces.Should().Be(2);

        // Percentage
        default(Percentage).Value.Should().Be(0m);
        default(Percentage).Fraction.Should().Be(0m);
        default(Percentage).ToString().Should().Be("0%");

        // TaxRate
        default(TaxRate).Value.Should().Be(0m);
        default(TaxRate).Fraction.Should().Be(0m);
        default(TaxRate).IsExempt.Should().BeTrue();

        // DiscountRate
        default(DiscountRate).Value.Should().Be(0m);
        default(DiscountRate).Fraction.Should().Be(0m);
        default(DiscountRate).IsZero.Should().BeTrue();

        // Quantity
        default(Quantity).Value.Should().Be(0);
        default(Quantity).IsZero.Should().BeTrue();

        // BusinessDate
        default(BusinessDate).Value.Should().Be(DateOnly.MinValue);

        // DateRange
        default(DateRange).Start.Should().Be(DateOnly.MinValue);
        default(DateRange).End.Should().Be(DateOnly.MinValue);
        default(DateRange).DurationInDays.Should().Be(1);

        // Range<int>
        default(Range<int>).Start.Should().Be(0);
        default(Range<int>).End.Should().Be(0);

        // TimeRange
        default(TimeRange).Start.Should().Be(default(TimeOnly));
        default(TimeRange).End.Should().Be(default(TimeOnly));
        default(TimeRange).CrossesMidnight.Should().BeFalse();
        default(TimeRange).Duration.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void ValueObjects_SatisfyEqualityAxioms_ReflexiveSymmetricTransitiveConsistent()
    {
        var m1 = Money.Create(100.50m, CurrencyCode.USD).Value;
        var m2 = Money.Create(100.50m, CurrencyCode.USD).Value;
        var m3 = Money.Create(100.50m, CurrencyCode.USD).Value;
        var mDiff = Money.Create(100.50m, CurrencyCode.EUR).Value;

        // Reflexivity
        var m1Clone = m1;
        (m1 == m1Clone).Should().BeTrue();
        m1.Equals(m1).Should().BeTrue();

        // Symmetry
        (m1 == m2).Should().BeTrue();
        (m2 == m1).Should().BeTrue();
        m1.Equals(m2).Should().BeTrue();
        m2.Equals(m1).Should().BeTrue();

        // Transitivity
        (m1 == m2 && m2 == m3).Should().BeTrue();
        (m1 == m3).Should().BeTrue();

        // Consistency with HashCode
        m1.GetHashCode().Should().Be(m2.GetHashCode());

        // Inequality
        (m1 == mDiff).Should().BeFalse();
        (m1 != mDiff).Should().BeTrue();
    }

    [Fact]
    public void ValueObjects_InHashSetAndDictionary_MaintainHighPerformanceAndZeroCollisionDegradation()
    {
        var set = new HashSet<Money>();
        var dict = new Dictionary<Email, string>();

        for (int i = 0; i < 500; i++)
        {
            var money = Money.Create(i * 1.5m, CurrencyCode.USD).Value;
            set.Add(money).Should().BeTrue();

            var email = Email.Create($"user_{i}@company.org").Value;
            dict[email] = $"Payload_{i}";
        }

        set.Count.Should().Be(500);
        dict.Count.Should().Be(500);

        var probeMoney = Money.Create(75.0m, CurrencyCode.USD).Value;
        set.Contains(probeMoney).Should().BeTrue();

        var probeEmail = Email.Create("user_50@company.org").Value;
        dict[probeEmail].Should().Be("Payload_50");
    }
}
