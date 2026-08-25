// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:OverrideMethodsOnComparableTypes")]
public readonly record struct TestCustomPoint(int Value, string Tag) : IComparable<TestCustomPoint>, IEquatable<TestCustomPoint>
{
    public int CompareTo(TestCustomPoint other) => Value.CompareTo(other.Value);
    public bool Equals(TestCustomPoint other) => Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}

