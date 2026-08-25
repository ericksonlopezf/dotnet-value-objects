// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Serialization.Json;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class TestNullableStringJsonConverter : SingleValueObjectJsonConverter<TestNullableStringVo, string>
{
    protected override Result<TestNullableStringVo> CreateInstance(string value) => TestNullableStringVo.Create(value);
}


