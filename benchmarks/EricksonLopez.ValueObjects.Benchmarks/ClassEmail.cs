// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects.Benchmarks;

public sealed class ClassEmail
{
    public string Value { get; }
    public ClassEmail(string value) => Value = value;
    public override bool Equals(object? obj) => obj is ClassEmail other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
