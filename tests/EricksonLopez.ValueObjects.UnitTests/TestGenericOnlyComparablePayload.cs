// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.UnitTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:OverrideMethodsOnComparableTypes")]
public sealed class TestGenericOnlyComparablePayload : IComparable<TestGenericOnlyComparablePayload>
{
    public int Value { get; }

    public TestGenericOnlyComparablePayload(int value) => Value = value;

    public int CompareTo(TestGenericOnlyComparablePayload? other) =>
        other is null ? 1 : Value.CompareTo(other.Value);
}
