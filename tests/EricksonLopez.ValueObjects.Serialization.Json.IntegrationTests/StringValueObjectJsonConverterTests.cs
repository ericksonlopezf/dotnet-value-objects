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

public sealed class StringValueObjectJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public StringValueObjectJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new TestTenantCodeStringJsonConverter());
    }

    [Fact]
    public void Read_WhenValidJsonStringProvided_ReturnsDeserializedValueObject()
    {
        var json = "\"acme-corp\"";
        var result = JsonSerializer.Deserialize<TenantCode>(json, _options);

        result.Should().NotBeNull();
        result!.Value.Should().Be("acme-corp");
    }

    [Fact]
    public void Read_WhenJsonNullTokenProvided_ReturnsNull()
    {
        var json = "null";
        var result = JsonSerializer.Deserialize<TenantCode>(json, _options);

        result.Should().BeNull();
    }

    [Fact]
    public void Read_WhenDomainValidationFails_ThrowsJsonException()
    {
        var invalidJson = "\"\""; // empty string fails TenantCode validation

        Action act = () => JsonSerializer.Deserialize<TenantCode>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("*Failed to deserialize 'TenantCode'*");
    }

    [Fact]
    public void Read_WhenDirectlyReadingNullToken_ReturnsNull()
    {
        var converter = new TestTenantCodeStringJsonConverter();
        var utf8Json = Encoding.UTF8.GetBytes("null");
        var reader = new Utf8JsonReader(utf8Json);
        reader.Read();

        var result = converter.Read(ref reader, typeof(TenantCode), _options);

        result.Should().BeNull();
    }

    [Fact]
    public void Write_WhenValidInstanceProvided_SerializesStringValue()
    {
        var vo = TenantCode.Create("enterprise-01").Value;
        var json = JsonSerializer.Serialize(vo, _options);

        json.Should().Be("\"enterprise-01\"");
    }

    [Fact]
    public void Write_WhenNullInstanceProvided_SerializesNull()
    {
        TenantCode? vo = null;
        var json = JsonSerializer.Serialize(vo, _options);

        json.Should().Be("null");
    }

    [Fact]
    public void Write_WhenNullPassedToConverter_WritesNullValueDirectly()
    {
        var converter = new TestTenantCodeStringJsonConverter();
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
        var converter = new TestTenantCodeStringJsonConverter();
        var vo = TenantCode.Create("enterprise-01").Value;

        Action act = () => converter.Write(null!, vo, _options);

        act.Should().Throw<ArgumentNullException>();
    }
}





