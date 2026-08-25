// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Serialization.Json;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class TestIntScalarJsonConverter : SingleValueObjectJsonConverter<TestIntScalarVo, int>
{
    protected override Result<TestIntScalarVo> CreateInstance(int value) => TestIntScalarVo.Create(value);
}


