// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AwesomeAssertions;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;
using EricksonLopez.ValueObjects.UnitTests;
using Xunit;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class RangeJsonConverterTests
{
    private readonly JsonSerializerOptions _options;

    public RangeJsonConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new RangeJsonConverter<int>());
    }

    [Fact]
    public void Read_WhenValidJsonObjectProvided_ReturnsRange()
    {
        var json = "{\"Start\": 10, \"End\": 50}";
        var range = JsonSerializer.Deserialize<Range<int>>(json, _options);

        range.Start.Should().Be(10);
        range.End.Should().Be(50);
    }

    [Fact]
    public void Read_WhenCaseInsensitivePropertyNamesProvided_HandlesCorrectly()
    {
        var json = "{\"start\": 5, \"end\": 25}";
        var range = JsonSerializer.Deserialize<Range<int>>(json, _options);

        range.Start.Should().Be(5);
        range.End.Should().Be(25);
    }

    [Fact]
    public void Read_WhenTokenIsNotStartObject_ThrowsJsonException()
    {
        var invalidJson = "[10, 50]";

        Action act = () => JsonSerializer.Deserialize<Range<int>>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("Expected StartObject token when deserializing Range.");
    }

    [Fact]
    public void Read_WhenStartPropertyIsMissing_ThrowsJsonException()
    {
        var invalidJson = "{\"End\": 50}";

        Action act = () => JsonSerializer.Deserialize<Range<int>>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("Range requires both 'Start' and 'End' properties.");
    }

    [Fact]
    public void Read_WhenEndPropertyIsMissing_ThrowsJsonException()
    {
        var invalidJson = "{\"Start\": 10}";

        Action act = () => JsonSerializer.Deserialize<Range<int>>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("Range requires both 'Start' and 'End' properties.");
    }

    [Fact]
    public void Read_WhenExtraPropertiesPresent_IgnoresAndDeserializesSuccessfully()
    {
        var json = "{\"Extra\": \"value\", \"Start\": 1, \"IgnoredProp\": 999, \"End\": 10}";
        var range = JsonSerializer.Deserialize<Range<int>>(json, _options);

        range.Start.Should().Be(1);
        range.End.Should().Be(10);
    }

    [Fact]
    public void Read_WhenStartIsGreaterThanEnd_ThrowsJsonException()
    {
        var invalidJson = "{\"Start\": 100, \"End\": 10}";

        Action act = () => JsonSerializer.Deserialize<Range<int>>(invalidJson, _options);

        act.Should().Throw<JsonException>()
            .WithMessage("*Invalid Range deserialization*");
    }

    [Fact]
    public void Read_WhenDeserializingMultipleRangesInArray_StopsAtEndObject()
    {
        var json = "[{\"Start\": 1, \"End\": 5}, {\"Start\": 10, \"End\": 20}]";
        var ranges = JsonSerializer.Deserialize<List<Range<int>>>(json, _options);

        ranges.Should().NotBeNull();
        ranges!.Should().HaveCount(2);
        ranges[0].Start.Should().Be(1);
        ranges[0].End.Should().Be(5);
        ranges[1].Start.Should().Be(10);
        ranges[1].End.Should().Be(20);
    }

    [Fact]
    public void Write_WhenValidRange_SerializesCorrectly()
    {
        var range = Range<int>.Create(10, 50).Value;
        var json = JsonSerializer.Serialize(range, _options);

        json.Should().Be("{\"Start\":10,\"End\":50}");
    }

    [Fact]
    public void Write_WhenWriterIsNull_ThrowsArgumentNullException()
    {
        var converter = new RangeJsonConverter<int>();
        var range = Range<int>.Create(10, 50).Value;

        Action act = () => converter.Write(null!, range, _options);

        act.Should().Throw<ArgumentNullException>();
    }
}



