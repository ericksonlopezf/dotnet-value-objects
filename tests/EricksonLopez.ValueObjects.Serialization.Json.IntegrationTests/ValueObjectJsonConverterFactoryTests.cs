// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;
using Xunit;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class ValueObjectJsonConverterFactoryTests
{
    private readonly JsonSerializerOptions _options;

    private static readonly JsonSerializerOptions NullReturningOptions = new()
    {
        Converters = { new ValueObjectJsonConverterFactory(), new NullReturningStringJsonConverter() }
    };

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
    public void CanConvert_NullType_ThrowsArgumentNullException()
    {
        var factory = new ValueObjectJsonConverterFactory();
        Action act = () => factory.CanConvert(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RegisterConverter_NullConverter_ThrowsArgumentNullException()
    {
        Action act = () => ValueObjectJsonConverterFactory.RegisterConverter<string>(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateConverter_NullType_ThrowsArgumentNullException()
    {
        var factory = new ValueObjectJsonConverterFactory();
        Action act = () => factory.CreateConverter(null!, _options);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateConverter_UnrelatedType_ReturnsNull()
    {
        var factory = new ValueObjectJsonConverterFactory();
        factory.CreateConverter(typeof(int), _options).Should().BeNull();
        factory.CreateConverter(typeof(string), _options).Should().BeNull();
    }

    [Fact]
    public void CreateConverter_CachesAllResolvedConverters()
    {
        var factory = new ValueObjectJsonConverterFactory();

        void AssertCached(Type type)
        {
            ValueObjectJsonConverterFactory.ClearCache();
            var c1 = factory.CreateConverter(type, _options);
            var c2 = factory.CreateConverter(type, _options);
            c1.Should().NotBeNull();
            c1.Should().BeSameAs(c2);
        }

        AssertCached(typeof(Money));
        AssertCached(typeof(CurrencyCode));
        AssertCached(typeof(Range<int>));
        AssertCached(typeof(Range<decimal>));
        AssertCached(typeof(Range<double>));
        AssertCached(typeof(Range<DateTime>));
        AssertCached(typeof(Range<DateOnly>));
        AssertCached(typeof(Range<long>));
        AssertCached(typeof(Email));
        AssertCached(typeof(Country));
    }

    [Fact]
    public void CreateConverter_ReturnsExactSpecializedConverterForEachRangeType()
    {
        ValueObjectJsonConverterFactory.ClearCache();
        var factory = new ValueObjectJsonConverterFactory();

        factory.CreateConverter(typeof(Range<int>), _options).Should().BeOfType<RangeJsonConverter<int>>();
        factory.CreateConverter(typeof(Range<decimal>), _options).Should().BeOfType<RangeJsonConverter<decimal>>();
        factory.CreateConverter(typeof(Range<double>), _options).Should().BeOfType<RangeJsonConverter<double>>();
        factory.CreateConverter(typeof(Range<DateTime>), _options).Should().BeOfType<RangeJsonConverter<DateTime>>();
        factory.CreateConverter(typeof(Range<DateOnly>), _options).Should().BeOfType<RangeJsonConverter<DateOnly>>();
        factory.CreateConverter(typeof(Range<long>), _options).Should().BeOfType<RangeJsonConverter<long>>();
    }

    [Fact]
    public void InnerConverters_NullSafetyAndEdgeCases()
    {
        var factory = new ValueObjectJsonConverterFactory();

        // CurrencyCode inner converter
        var currConv = (JsonConverter<CurrencyCode>)factory.CreateConverter(typeof(CurrencyCode), _options)!;
        Action actCurrNullWriter = () => currConv.Write(null!, CurrencyCode.USD, _options);
        actCurrNullWriter.Should().Throw<ArgumentNullException>();

        using var stream1 = new System.IO.MemoryStream();
        using (var writer1 = new Utf8JsonWriter(stream1))
        {
            currConv.Write(writer1, default, _options);
        }
        System.Text.Encoding.UTF8.GetString(stream1.ToArray()).Should().Be("null");

        // Parsable inner converter
        var parsableConv = (JsonConverter<Email>)factory.CreateConverter(typeof(Email), _options)!;
        Action actParsableNullWriter = () => parsableConv.Write(null!, Email.Create("a@b.com").Value, _options);
        actParsableNullWriter.Should().Throw<ArgumentNullException>();

        var classParsableConv = (JsonConverter<TestParsableClass>)factory.CreateConverter(typeof(TestParsableClass), _options)!;
        using var stream2 = new System.IO.MemoryStream();
        using (var writer2 = new Utf8JsonWriter(stream2))
        {
            classParsableConv.Write(writer2, null!, _options);
        }
        System.Text.Encoding.UTF8.GetString(stream2.ToArray()).Should().Be("null");


        // ReflectionSingleValueObject inner converter (Country)
        var svoConv = (JsonConverter<Country>)factory.CreateConverter(typeof(Country), _options)!;
        Action actSvoNullWriter = () => svoConv.Write(null!, Country.Create("US").Value, _options);
        actSvoNullWriter.Should().Throw<ArgumentNullException>();

        using var stream3 = new System.IO.MemoryStream();
        using (var writer3 = new Utf8JsonWriter(stream3))
        {
            svoConv.Write(writer3, null!, _options);
        }
        System.Text.Encoding.UTF8.GetString(stream3.ToArray()).Should().Be("null");

        // SingleValueObject deserialize null token
        var nullSvo = JsonSerializer.Deserialize<Country>("null", _options);
        nullSvo.Should().BeNull();

        // SingleValueObject deserialize invalid value throws JsonException with message
        Action actInvalidSvo = () => JsonSerializer.Deserialize<Country>("\"INVALID_LONG_CODE\"", _options);
        actInvalidSvo.Should().Throw<JsonException>().WithMessage("*Cannot instantiate 'Country' from*");
    }

    [Fact]
    public void Money_EdgeCasesAndErrors_HandledCorrectly()
    {
        // Empty string -> default
        var fromEmpty = JsonSerializer.Deserialize<Money>("\"\"", _options);
        fromEmpty.Should().Be(default);

        // Invalid JSON token (array instead of object or string)
        Action actArray = () => JsonSerializer.Deserialize<Money>("[1, 2]", _options);
        actArray.Should().Throw<JsonException>()
            .WithMessage("Expected StartObject or String token when deserializing Money, got 'StartArray'.");

        // Invalid JSON token (number instead of object or string)
        Action actNumber = () => JsonSerializer.Deserialize<Money>("12345", _options);
        actNumber.Should().Throw<JsonException>()
            .WithMessage("Expected StartObject or String token when deserializing Money, got 'Number'.");

        // Missing amount
        Action actNoAmount = () => JsonSerializer.Deserialize<Money>("{\"currency\": \"USD\"}", _options);
        actNoAmount.Should().Throw<JsonException>()
            .WithMessage("Money object requires an 'amount' property.");

        // Missing currency
        Action actNoCurr = () => JsonSerializer.Deserialize<Money>("{\"amount\": 100}", _options);
        actNoCurr.Should().Throw<JsonException>()
            .WithMessage("Money object requires a 'currency' property.");

        // Invalid currency
        Action actBadCurr = () => JsonSerializer.Deserialize<Money>("{\"amount\": 100, \"currency\": \"NOT_VALID\"}", _options);
        actBadCurr.Should().Throw<JsonException>()
            .WithMessage("*Invalid currency code 'NOT_VALID'*");

        // Unknown extra nested properties are safely skipped
        var withExtra = JsonSerializer.Deserialize<Money>("{\"nested\": {\"foo\": \"bar\", \"arr\": [1, 2]}, \"amount\": 100, \"currency\": \"USD\"}", _options);
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

        // Amount out of range in MoneyJsonConverter
        Action actOutOfRange = () => JsonSerializer.Deserialize<Money>("{\"amount\": 1000000000000000000, \"currency\": \"USD\"}", _options);
        actOutOfRange.Should().Throw<JsonException>()
            .WithMessage("*Failed to deserialize Money*");

        // Null SingleValueObject deserialization
        var nullCountry = JsonSerializer.Deserialize<Country>("null", _options);
        nullCountry.Should().BeNull();

        // Abstract SingleValueObject converter instantiation
        var factory = new ValueObjectJsonConverterFactory();
        var genericConverter = factory.CreateConverter(typeof(SingleValueObject<Country, string>), _options);
        genericConverter.Should().NotBeNull();

        // Underlying converter returning null
        var nullVo = JsonSerializer.Deserialize<Country>("\"some-string\"", NullReturningOptions);
        nullVo.Should().BeNull();
    }

    [Fact]
    public void SingleValueObject_DirectAbstractType_RoundTrip()
    {
        string json = "\"US\"";
        var deserialized = JsonSerializer.Deserialize<SingleValueObject<Country, string>>(json, _options);
        deserialized.Should().NotBeNull();
        deserialized!.Value.Should().Be("US");

        var nullResult = JsonSerializer.Deserialize<SingleValueObject<Country, string>>("null", _options);
        nullResult.Should().BeNull();
    }

    [Fact]
    public void SingleValueObject_IntValueType_DeserializesNullToken()
    {
        var nullResult = JsonSerializer.Deserialize<TestIntScalarVo>("null", _options);
        nullResult.Should().BeNull();

        var directNull = JsonSerializer.Deserialize<SingleValueObject<TestIntScalarVo, int>>("null", _options);
        directNull.Should().BeNull();
    }

    [Fact]
    public void CanConvert_WhenTypeImplementsNonParsableGenericInterface_ReturnsFalse()
    {
        var factory = new ValueObjectJsonConverterFactory();
        factory.CanConvert(typeof(TestNonParsableGenericVo)).Should().BeFalse();
        factory.CreateConverter(typeof(TestNonParsableGenericVo), _options).Should().BeNull();
    }

    private sealed record TestNonParsableGenericVo : IComparable<TestNonParsableGenericVo>
    {
        public int CompareTo(TestNonParsableGenericVo? other) => 0;
    }

    private sealed class SampleOrderDto
    {
        public Money Total { get; set; }
        public Range<int> ValidPeriod { get; set; }
        public Email CustomerEmail { get; set; }
    }

    private sealed class TestParsableClass : IParsable<TestParsableClass>
    {
        public static TestParsableClass Parse(string s, IFormatProvider? provider) => new();
        public static bool TryParse(string? s, IFormatProvider? provider, out TestParsableClass result)
        {
            result = new();
            return true;
        }
        public override string ToString() => "TestParsable";
    }
}

