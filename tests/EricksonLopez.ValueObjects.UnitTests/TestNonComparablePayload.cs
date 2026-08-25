// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;
namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class TestNonComparablePayload
{
    public string Data { get; }

    public TestNonComparablePayload(string data)
    {
        Data = data;
    }
}


