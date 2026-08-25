// Copyright © Erickson Lopez. MIT License.
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using AwesomeAssertions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class SingleValueObjectJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public SingleValueObjectJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new TestIntScalarJsonConverter());
    }

    [Fact]
    public void Read_WhenValidPrimitiveProvided_ReturnsDeserializedValueObject()
    {
        var json = "42";
        var result = JsonSerializer.Deserialize<TestIntScalarVo>(json, _options);

        result.Should().NotBeNull();
        result!.Value.Should().Be(42);
    }

    [Fact]
    public void Read_WhenJsonNullTokenProvided_ReturnsNull()
    {
        var json = "null";
        var result = JsonSerializer.Deserialize<TestIntScalarVo>(json, _options);

        result.Should().BeNull();
    }

    [Fact]
    public void Read_WhenDomainValidationFails_ThrowsJsonException()
    {
        var invalidJson = "-5";

        Action act = () => JsonSerializer.Deserialize<TestIntScalarVo>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("*Failed to deserialize 'TestIntScalarVo'*");
    }

    [Fact]
    public void Read_WhenDirectlyReadingNullToken_ReturnsNull()
    {
        var converter = new TestIntScalarJsonConverter();
        var utf8Json = Encoding.UTF8.GetBytes("null");
        var reader = new Utf8JsonReader(utf8Json);
        reader.Read();

        var result = converter.Read(ref reader, typeof(TestIntScalarVo), _options);

        result.Should().BeNull();
    }

    [Fact]
    public void Read_WhenUnderlyingConverterReturnsNull_ReturnsNull()
    {
        var customOptions = new JsonSerializerOptions();
        customOptions.Converters.Add(new NullReturningStringJsonConverter());
        customOptions.Converters.Add(new TestNullableStringJsonConverter());

        var converter = new TestNullableStringJsonConverter();
        var utf8Json = Encoding.UTF8.GetBytes("\"some-string\"");
        var reader = new Utf8JsonReader(utf8Json);
        reader.Read();

        var result = converter.Read(ref reader, typeof(TestNullableStringVo), customOptions);

        result.Should().BeNull();
    }

    [Fact]
    public void Write_WhenValidInstanceProvided_SerializesPrimitive()
    {
        var vo = TestIntScalarVo.Create(100).Value;
        var json = JsonSerializer.Serialize(vo, _options);

        json.Should().Be("100");
    }

    [Fact]
    public void Write_WhenNullInstanceProvided_SerializesNull()
    {
        TestIntScalarVo? vo = null;
        var json = JsonSerializer.Serialize(vo, _options);

        json.Should().Be("null");
    }

    [Fact]
    public void Write_WhenNullPassedToConverter_WritesNullValueDirectly()
    {
        var converter = new TestIntScalarJsonConverter();
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            converter.Write(writer, null!, _options);
        }

        var json = Encoding.UTF8.GetString(stream.ToArray());
        json.Should().Be("null");
    }

    [Fact]
    public void Write_WhenWriterIsNull_ThrowsArgumentNullException()
    {
        var converter = new TestIntScalarJsonConverter();
        var vo = TestIntScalarVo.Create(100).Value;

        Action act = () => converter.Write(null!, vo, _options);

        act.Should().Throw<ArgumentNullException>();
    }
}





