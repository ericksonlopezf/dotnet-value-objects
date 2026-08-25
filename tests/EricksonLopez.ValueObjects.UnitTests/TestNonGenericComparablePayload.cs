// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.ValueObjects;
using EricksonLopez.ValueObjects.UnitTests;

namespace EricksonLopez.ValueObjects.UnitTests;

public sealed class TestNonGenericComparablePayload : IComparable
{
    public int Number { get; }

    public TestNonGenericComparablePayload(int number)
    {
        Number = number;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is TestNonGenericComparablePayload other) return Number.CompareTo(other.Number);
        throw new ArgumentException("Object is not TestNonGenericComparablePayload");
    }

    public override bool Equals(object? obj) => obj is TestNonGenericComparablePayload other && Number == other.Number;
    public override int GetHashCode() => Number.GetHashCode();

    public static bool operator ==(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        left?.Number == right?.Number;
    public static bool operator !=(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        !(left == right);
    public static bool operator <(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        left is null ? right is not null : left.CompareTo(right) < 0;
    public static bool operator <=(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        left is null || left.CompareTo(right) <= 0;
    public static bool operator >(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        left is not null && left.CompareTo(right) > 0;
    public static bool operator >=(TestNonGenericComparablePayload? left, TestNonGenericComparablePayload? right) =>
        left is null ? right is null : left.CompareTo(right) >= 0;
}


