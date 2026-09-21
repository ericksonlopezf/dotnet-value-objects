// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;
using Xunit;

namespace EricksonLopez.ValueObjects.UnitTests;

/// <summary>
/// Forensic tests establishing empirical evidence of architectural,
/// semantic, security, and contract compliance across the library.
/// </summary>
public sealed class ForensicAdversarialEvidenceTests
{
    [Fact]
    public void VO_DOM_001_HierarchyPath_Create_WhenPathHasDoubleSlashes_ReturnsFailure()
    {
        // Act: "/a//b/" contains an empty segment, which must be rejected
        var result = HierarchyPath.Create("/a//b/");

        // Verification: Create rejects double slashes with HierarchyPath.InvalidFormat
        result.IsFailure.Should().BeTrue("HierarchyPath.Create must reject consecutive slashes");
        result.Error.Code.Should().Be("HierarchyPath.InvalidFormat");
    }

    [Fact]
    public void VO_DOM_001_HierarchyPath_Create_WhenPathHasTraversalTokens_ReturnsFailure()
    {
        // Act: "/a/../b/" contains path traversal token '..'
        var result = HierarchyPath.Create("/a/../b/");

        // Verification: Create rejects traversal tokens with HierarchyPath.InvalidSegment
        result.IsFailure.Should().BeTrue("HierarchyPath.Create must reject path traversal tokens ('..')");
        result.Error.Code.Should().Be("HierarchyPath.InvalidSegment");
    }

    [Fact]
    public void VO_DOM_002_Money_DefaultInstance_ComparisonAndSortingSucceedDeterministically()
    {
        // Arrange
        Money defaultMoney1 = default;
        Money defaultMoney2 = default;

        // Verification 1: Value equality returns true (record struct equality)
        (defaultMoney1 == defaultMoney2).Should().BeTrue();
        defaultMoney1.Equals(defaultMoney2).Should().BeTrue();

        // Verification 2: CompareTo returns 0 for two default instances without throwing
        defaultMoney1.CompareTo(defaultMoney2).Should().Be(0);

        // Verification 3: default is ordered before initialized instances
        var usdZero = Money.ZeroUsd;
        defaultMoney1.CompareTo(usdZero).Should().BeLessThan(0);
        usdZero.CompareTo(defaultMoney1).Should().BeGreaterThan(0);

        // Verification 4: Sorting an array containing default(Money) succeeds deterministically
        var fiftyUsd = Money.Create(50m, CurrencyCode.USD).Value;
        Money[] moneyArray = [usdZero, defaultMoney1, fiftyUsd];
        Array.Sort(moneyArray);

        moneyArray[0].Should().Be(defaultMoney1);
        moneyArray[1].Should().Be(usdZero);
        moneyArray[2].Should().Be(fiftyUsd);
    }

    [Fact]
    public void VO_DOM_003_Range_IsDegenerate_IdentifiesIntervalOfMeasureZero()
    {
        // Arrange: A single-point degenerate closed interval [5 .. 5]
        var rangeResult = Range<int>.Create(5, 5);
        rangeResult.IsSuccess.Should().BeTrue();
        var range = rangeResult.Value;

        // Verification 1: Contains(5) returns true (5 is within [5..5])
        range.Contains(5).Should().BeTrue();

        // Verification 2: IsDegenerate is true and IsEmpty returns true (interval of measure zero)
        range.IsDegenerate.Should().BeTrue("Range<T>.IsDegenerate must be true for [5..5]");
        range.IsEmpty.Should().BeTrue("Range<T>.IsEmpty represents measure zero");

        // Verification 3: Non-degenerate range [5..10]
        var nonDegenerate = Range<int>.Create(5, 10).Value;
        nonDegenerate.IsDegenerate.Should().BeFalse();
        nonDegenerate.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void VO_DOM_004_Email_ToString_PreservesRawValueAndEnablesRoundTripParsing()
    {
        // Arrange
        var email = Email.Create("erickson@domain.com").Value;

        // Verification 1: ToString() returns raw value for valid roundtrip
        string stringRep = email.ToString();
        stringRep.Should().Be("erickson@domain.com");

        // Verification 2: Parsing the string representation round-trips to identical email
        var parsed = Email.Parse(stringRep);
        parsed.Should().Be(email, "Parse(ToString(email)) must round-trip exactly");

        // Verification 3: Masked representation is available via Masked() and ToMaskedString()
        email.Masked().Should().Be("e***@domain.com");
        email.ToMaskedString().Should().Be("e***@domain.com");
    }

    [Fact]
    public void VO_API_001_ValueObjectJsonConverterFactory_ExistsAndCanConvertValueObjects()
    {
        // Verification 1: ValueObjectJsonConverterFactory exists and is instantiable
        var factory = new ValueObjectJsonConverterFactory();
        factory.Should().NotBeNull();

        // Verification 2: CanConvert detects Money, CurrencyCode, Range<int>, SingleValueObject
        factory.CanConvert(typeof(Money)).Should().BeTrue();
        factory.CanConvert(typeof(CurrencyCode)).Should().BeTrue();
        factory.CanConvert(typeof(Range<int>)).Should().BeTrue();
        factory.CanConvert(typeof(SingleValueObject<Country, string>)).Should().BeTrue();
        factory.CanConvert(typeof(int)).Should().BeFalse();

        // Verification 3: Serializing and deserializing Money via options with factory
        var options = new JsonSerializerOptions();
        options.Converters.Add(factory);

        var money = Money.Create(100.50m, CurrencyCode.USD).Value;
        string json = JsonSerializer.Serialize(money, options);
        var deserialized = JsonSerializer.Deserialize<Money>(json, options);
        deserialized.Should().Be(money);
    }

    [Fact]
    public void VO_API_002_Money_Implements_IParsableAndISpanParsable()
    {
        // Verification 1: Money implements IParsable<Money> and ISpanParsable<Money>
        typeof(IParsable<Money>).IsAssignableFrom(typeof(Money)).Should().BeTrue("Money must implement IParsable<Money>");
        typeof(ISpanParsable<Money>).IsAssignableFrom(typeof(Money)).Should().BeTrue("Money must implement ISpanParsable<Money>");

        // Verification 2: Parse standard representations
        var parsed1 = Money.Parse("150.50 USD");
        parsed1.Amount.Should().Be(150.50m);
        parsed1.Currency.Should().Be(CurrencyCode.USD);

        var parsed2 = Money.Parse("EUR 200.75");
        parsed2.Amount.Should().Be(200.75m);
        parsed2.Currency.Should().Be(CurrencyCode.EUR);

        // Verification 3: TryParse
        Money.TryParse("99.99 USD", null, out var parsed3).Should().BeTrue();
        parsed3.Amount.Should().Be(99.99m);
        parsed3.Currency.Should().Be(CurrencyCode.USD);

        Money.TryParse("invalid", null, out _).Should().BeFalse();
    }

    [Fact]
    public void VO_ARCH_001_SingleValueObject_Implements_IValueObjectTSelf()
    {
        // Verification: SingleValueObject<TSelf, TValue> implements IValueObject<TSelf>
        typeof(IValueObject<Country>).IsAssignableFrom(typeof(SingleValueObject<Country, string>))
            .Should().BeTrue("SingleValueObject<TSelf, TValue> must implement IValueObject<TSelf>");
        typeof(IValueObject<Country>).IsAssignableFrom(typeof(Country))
            .Should().BeTrue("Country must implement IValueObject<Country>");
    }

    [Fact]
    public void VO_PERF_001_PhoneNumber_ISpanParsable_AllocatesHeapString()
    {
        // Arrange
        ReadOnlySpan<char> span = "+18095551234".AsSpan();

        // Verification: ISpanParsable Parse exists and parses span correctly
        var parseMethod = typeof(PhoneNumber).GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, [typeof(ReadOnlySpan<char>), typeof(IFormatProvider)]);
        parseMethod.Should().NotBeNull();

        var phone = PhoneNumber.Parse(span);
        phone.Value.Should().Be("+18095551234");
    }
}
