// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.Serialization.Json;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.Serialization.Json.IntegrationTests;

public sealed class TestTenantCodeStringJsonConverter : StringValueObjectJsonConverter<TenantCode>
{
    protected override Result<TenantCode> CreateInstance(string value) => TenantCode.Create(value);
}


