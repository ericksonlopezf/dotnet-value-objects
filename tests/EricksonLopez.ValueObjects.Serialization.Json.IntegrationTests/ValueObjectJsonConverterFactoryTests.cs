// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.Json;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;
using Xunit;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class ValueObjectJsonConverterFactoryTests
{
    private readonly JsonSerializerOptions _options;

    public ValueObjectJsonConverterFactoryTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ValueObjectJsonConverterFactory());
    }

    [Fact]
    public void CanConvert_SupportsAllTargetTypes()
    {
        var factory = new ValueObjectJsonConverterFactory();

        factory.CanConvert(typeof(Money)).Should().BeTrue();
        factory.CanConvert(typeof(CurrencyCode)).Should().BeTrue();
        factory.CanConvert(typeof(Range<int>)).Should().BeTrue();
        factory.CanConvert(typeof(SingleValueObject<Country, string>)).Should().BeTrue();
        factory.CanConvert(typeof(Country)).Should().BeTrue();
        factory.CanConvert(typeof(Email)).Should().BeTrue();
        factory.CanConvert(typeof(PhoneNumber)).Should().BeTrue();

        factory.CanConvert(typeof(int)).Should().BeFalse();
        factory.CanConvert(typeof(string)).Should().BeFalse();
        factory.CanConvert(typeof(DateTime)).Should().BeFalse();
    }

    [Fact]
    public void Money_RoundTrip_SerializesAndDeserializes()
    {
        var money = Money.Create(1500.50m, CurrencyCode.USD).Value;

        string json = JsonSerializer.Serialize(money, _options);
        json.Should().Contain("\"amount\":1500.5");
        json.Should().Contain("\"currency\":\"USD\"");

        var deserialized = JsonSerializer.Deserialize<Money>(json, _options);
        deserialized.Should().Be(money);
        deserialized.Amount.Should().Be(1500.50m);
        deserialized.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Money_FromStringFormat_DeserializesSuccessfully()
    {
        string json = "\"1500.50 USD\"";

        var deserialized = JsonSerializer.Deserialize<Money>(json, _options);
        deserialized.Amount.Should().Be(1500.50m);
        deserialized.Currency.Should().Be(CurrencyCode.USD);
    }

    [Fact]
    public void Money_FromReverseStringFormat_DeserializesSuccessfully()
    {
        string json = "\"EUR 250.00\"";

        var deserialized = JsonSerializer.Deserialize<Money>(json, _options);
        deserialized.Amount.Should().Be(250.00m);
        deserialized.Currency.Should().Be(CurrencyCode.EUR);
    }

    [Fact]
    public void Range_RoundTrip_SerializesAndDeserializes()
    {
        var range = Range<int>.Create(10, 50).Value;

        string json = JsonSerializer.Serialize(range, _options);
        json.Should().Contain("\"Start\":10");
        json.Should().Contain("\"End\":50");

        var deserialized = JsonSerializer.Deserialize<Range<int>>(json, _options);
        deserialized.Should().Be(range);
        deserialized.Start.Should().Be(10);
        deserialized.End.Should().Be(50);
    }

    [Fact]
    public void CurrencyCode_RoundTrip_SerializesAndDeserializes()
    {
        var currency = CurrencyCode.USD;

        string json = JsonSerializer.Serialize(currency, _options);
        json.Should().Be("\"USD\"");

        var deserialized = JsonSerializer.Deserialize<CurrencyCode>(json, _options);
        deserialized.Should().Be(currency);
    }

    [Fact]
    public void Email_RoundTrip_SerializesAndDeserializes()
    {
        var email = Email.Create("user@domain.com").Value;

        string json = JsonSerializer.Serialize(email, _options);
        json.Should().Be("\"user@domain.com\"");

        var deserialized = JsonSerializer.Deserialize<Email>(json, _options);
        deserialized.Should().Be(email);
    }

    [Fact]
    public void ComplexDto_WithMultipleValueObjects_SerializesAndDeserializes()
    {
        var dto = new SampleOrderDto
        {
            Total = Money.Create(99.95m, CurrencyCode.USD).Value,
            ValidPeriod = Range<int>.Create(1, 12).Value,
            CustomerEmail = Email.Create("customer@store.com").Value
        };

        string json = JsonSerializer.Serialize(dto, _options);
        var deserialized = JsonSerializer.Deserialize<SampleOrderDto>(json, _options);

        deserialized.Should().NotBeNull();
        deserialized!.Total.Should().Be(dto.Total);
        deserialized.ValidPeriod.Should().Be(dto.ValidPeriod);
        deserialized.CustomerEmail.Should().Be(dto.CustomerEmail);
    }

    [Fact]
    public void RangeDecimal_RoundTrip_UsesDirectConverter()
    {
        var range = Range<decimal>.Create(1.5m, 99.9m).Value;
        string json = JsonSerializer.Serialize(range, _options);
        var deserialized = JsonSerializer.Deserialize<Range<decimal>>(json, _options);
        deserialized.Should().Be(range);
    }

    [Fact]
    public void RegisterConverter_PreRegistersCustomConverterForAot()
    {
        ValueObjectJsonConverterFactory.RegisterConverter(new MoneyJsonConverter());
        var money = Money.Create(500m, CurrencyCode.USD).Value;
        string json = JsonSerializer.Serialize(money, _options);
        var deserialized = JsonSerializer.Deserialize<Money>(json, _options);
        deserialized.Should().Be(money);
    }

    [Fact]
    public void AddValueObjectConverters_RegistersConverterSuccessfully()
    {
        var options = new JsonSerializerOptions();
        options.AddValueObjectConverters();
        options.Converters.Should().ContainSingle(c => c is ValueObjectJsonConverterFactory);

        // Calling a second time should not duplicate
        options.AddValueObjectConverters();
        options.Converters.Should().ContainSingle(c => c is ValueObjectJsonConverterFactory);
    }

    [Fact]
    public void AddValueObjectConverters_NullOptions_ThrowsArgumentNullException()
    {
        JsonSerializerOptions? nullOptions = null;
        Action act = () => nullOptions!.AddValueObjectConverters();
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Country_SingleValueObject_SerializesAndDeserializes()
    {
        var country = Country.Create("US").Value;
        string json = JsonSerializer.Serialize(country, _options);
        json.Should().Be("\"US\"");

        var deserialized = JsonSerializer.Deserialize<Country>(json, _options);
        deserialized.Should().Be(country);

        // Null round-trip
        string nullJson = JsonSerializer.Serialize<Country>(null!, _options);
        nullJson.Should().Be("null");
        var deserializedNull = JsonSerializer.Deserialize<Country>("null", _options);
        deserializedNull.Should().BeNull();
    }

    [Fact]
    public void Country_SingleValueObject_InvalidValue_ThrowsJsonException()
    {
        Action act = () => JsonSerializer.Deserialize<Country>("\"INVALID_LONG_CODE\"", _options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Range_Double_SerializesAndDeserializes()
    {
        var range = Range<double>.Create(1.5, 9.9).Value;
        string json = JsonSerializer.Serialize(range, _options);
        var deserialized = JsonSerializer.Deserialize<Range<double>>(json, _options);
        deserialized.Should().Be(range);
    }

    [Fact]
    public void Range_DateTime_SerializesAndDeserializes()
    {
        var now = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var range = Range<DateTime>.Create(now, now.AddDays(7)).Value;
        string json = JsonSerializer.Serialize(range, _options);
        var deserialized = JsonSerializer.Deserialize<Range<DateTime>>(json, _options);
        deserialized.Should().Be(range);
    }

    [Fact]
    public void Range_DateOnly_SerializesAndDeserializes()
    {
        var range = Range<DateOnly>.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 31)).Value;
        string json = JsonSerializer.Serialize(range, _options);
        var deserialized = JsonSerializer.Deserialize<Range<DateOnly>>(json, _options);
        deserialized.Should().Be(range);
    }

    [Fact]
    public void Range_Long_GenericBranch_SerializesAndDeserializes()
    {
        var range = Range<long>.Create(100L, 500L).Value;
        string json = JsonSerializer.Serialize(range, _options);
        var deserialized = JsonSerializer.Deserialize<Range<long>>(json, _options);
        deserialized.Should().Be(range);
    }

    [Fact]
    public void CurrencyCode_NullOrWhitespace_HandlesDefaults()
    {
        var fromNull = JsonSerializer.Deserialize<CurrencyCode>("null", _options);
        fromNull.IsInitialized.Should().BeFalse();

        var fromEmpty = JsonSerializer.Deserialize<CurrencyCode>("\"\"", _options);
        fromEmpty.IsInitialized.Should().BeFalse();

        string serializedDefault = JsonSerializer.Serialize(default(CurrencyCode), _options);
        serializedDefault.Should().Be("null");
    }

    [Fact]
    public void ParsableJsonConverter_NullHandling()
    {
        var fromNull = JsonSerializer.Deserialize<Email>("null", _options);
        fromNull.IsInitialized.Should().BeFalse();

        string serializedNull = JsonSerializer.Serialize((Email?)null, _options);
        serializedNull.Should().Be("null");
    }

    [Fact]
    public void Money_EdgeCasesAndErrors_HandledCorrectly()
    {
        // Empty string -> default
        var fromEmpty = JsonSerializer.Deserialize<Money>("\"\"", _options);
        fromEmpty.Should().Be(default);

        // Invalid JSON token (array instead of object or string)
        Action actArray = () => JsonSerializer.Deserialize<Money>("[1, 2]", _options);
        actArray.Should().Throw<JsonException>();

        // Missing amount
        Action actNoAmount = () => JsonSerializer.Deserialize<Money>("{\"currency\": \"USD\"}", _options);
        actNoAmount.Should().Throw<JsonException>();

        // Missing currency
        Action actNoCurr = () => JsonSerializer.Deserialize<Money>("{\"amount\": 100}", _options);
        actNoCurr.Should().Throw<JsonException>();

        // Invalid currency
        Action actBadCurr = () => JsonSerializer.Deserialize<Money>("{\"amount\": 100, \"currency\": \"NOT_VALID\"}", _options);
        actBadCurr.Should().Throw<JsonException>();

        // Unknown extra properties are safely skipped
        var withExtra = JsonSerializer.Deserialize<Money>("{\"amount\": 100, \"currency\": \"USD\", \"unknown\": true}", _options);
        withExtra.Amount.Should().Be(100);
        withExtra.Currency.Should().Be(CurrencyCode.USD);

        // String representation deserialization
        var fromString = JsonSerializer.Deserialize<Money>("\"100.50 USD\"", _options);
        fromString.Amount.Should().Be(100.50m);
        fromString.Currency.Should().Be(CurrencyCode.USD);

        // Null writer check
        var converter = new MoneyJsonConverter();
        Action actNullWriter = () => converter.Write(null!, fromString, _options);
        actNullWriter.Should().Throw<ArgumentNullException>();

        // Default money serialization
        var defaultMoneyJson = JsonSerializer.Serialize(default(Money), _options);
        defaultMoneyJson.Should().Contain("\"currency\":\"\"");
    }

    private sealed class SampleOrderDto
    {
        public Money Total { get; set; }
        public Range<int> ValidPeriod { get; set; }
        public Email CustomerEmail { get; set; }
    }
}
