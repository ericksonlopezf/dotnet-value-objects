// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects.Benchmarks;

public readonly struct StructEmail : IEquatable<StructEmail>
{
    public string Value { get; }
    public StructEmail(string value) => Value = value;
    public bool Equals(StructEmail other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is StructEmail other && Equals(other);
    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public static bool operator ==(StructEmail left, StructEmail right) => left.Equals(right);
    public static bool operator !=(StructEmail left, StructEmail right) => !left.Equals(right);
}
