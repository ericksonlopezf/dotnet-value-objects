// Copyright © Erickson Lopez. MIT License.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Provides a base record for value objects encapsulating a single underlying value.
/// </summary>
/// <remarks>
/// Encapsulates structural value-based equality, type safety, formatted output, and domain invariant infrastructure.
/// </remarks>
/// <typeparam name="TSelf">
/// The concrete value object type deriving from this base.
/// <para>
/// This type parameter is annotated with
/// <see cref="System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All"/> to ensure full
/// Native AOT and trimmer compatibility. Deriving types must be concrete, non-abstract classes or records.
/// </para>
/// </typeparam>
/// <typeparam name="TValue">The underlying primitive or complex value type.</typeparam>
[System.Diagnostics.DebuggerDisplay("{" + nameof(ToString) + "()}")]
public abstract record SingleValueObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TSelf, TValue> : IValueObject, IComparable<TSelf>, IComparable
    where TSelf : SingleValueObject<TSelf, TValue>
    where TValue : notnull
{
    /// <summary>
    /// Gets a value indicating whether the underlying value contains sensitive data requiring masking.
    /// </summary>
    protected virtual bool IsSensitive => false;

    /// <summary>
    /// Gets the mask string used when formatting sensitive values.
    /// </summary>
    protected virtual string Mask => "***";

    /// <summary>
    /// Gets the encapsulated underlying value.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SingleValueObject{TSelf, TValue}"/> class.
    /// </summary>
    /// <param name="value">The underlying value to encapsulate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
    protected SingleValueObject(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        Value = value;
    }

    /// <summary>
    /// Determines whether the current instance is equal to another instance of the same type based on value equality.
    /// </summary>
    /// <param name="other">The other value object to compare with this instance.</param>
    /// <returns><see langword="true"/> if the underlying values are equal; otherwise, <see langword="false"/>.</returns>
    public virtual bool Equals(TSelf? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    /// <inheritdoc/>
    public override int GetHashCode() => EqualityComparer<TValue>.Default.GetHashCode(Value);

    /// <summary>
    /// Compares the current instance with another instance of the same type.
    /// </summary>
    /// <param name="other">The other value object to compare with this instance.</param>
    /// <returns>A value indicating the relative order of the instances being compared.</returns>
    /// <exception cref="NotSupportedException">The underlying type <typeparamref name="TValue"/> does not implement <see cref="IComparable{T}"/> or <see cref="IComparable"/></exception>
    public int CompareTo(TSelf? other)
    {
        if (other is null) return 1;
        if (ReferenceEquals(this, other)) return 0;

        if (Value is IComparable<TValue> comparable)
        {
            return comparable.CompareTo(other.Value);
        }

        if (Value is IComparable nonGenericComparable)
        {
            return nonGenericComparable.CompareTo(other.Value);
        }

        throw new NotSupportedException($"Underlying type '{typeof(TValue).FullName}' does not implement IComparable.");
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not of type <typeparamref name="TSelf"/></exception>
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is TSelf other) return CompareTo(other);
        throw new ArgumentException($"Object must be of type {typeof(TSelf).Name}", nameof(obj));
    }

    /// <summary>
    /// Determines whether the left operand is less than the right operand.
    /// </summary>
    /// <param name="left">The left value object to compare.</param>
    /// <param name="right">The right value object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(SingleValueObject<TSelf, TValue>? left, SingleValueObject<TSelf, TValue>? right) =>
        left is null ? right is not null : left.CompareTo(right as TSelf) < 0;

    /// <summary>
    /// Determines whether the left operand is less than or equal to the right operand.
    /// </summary>
    /// <param name="left">The left value object to compare.</param>
    /// <param name="right">The right value object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(SingleValueObject<TSelf, TValue>? left, SingleValueObject<TSelf, TValue>? right) =>
        left is null || left.CompareTo(right as TSelf) <= 0;

    /// <summary>
    /// Determines whether the left operand is greater than the right operand.
    /// </summary>
    /// <param name="left">The left value object to compare.</param>
    /// <param name="right">The right value object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(SingleValueObject<TSelf, TValue>? left, SingleValueObject<TSelf, TValue>? right) =>
        left is not null && left.CompareTo(right as TSelf) > 0;

    /// <summary>
    /// Determines whether the left operand is greater than or equal to the right operand.
    /// </summary>
    /// <param name="left">The left value object to compare.</param>
    /// <param name="right">The right value object to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(SingleValueObject<TSelf, TValue>? left, SingleValueObject<TSelf, TValue>? right) =>
        left is null ? right is null : left.CompareTo(right as TSelf) >= 0;

    /// <inheritdoc/>
    public sealed override string ToString() => ToStringCore();

    /// <summary>
    /// Formats the string representation of the value object, applying masking when configured.
    /// </summary>
    /// <returns>The string representation of the wrapped value, or the mask string if marked sensitive.</returns>
    protected virtual string ToStringCore()
    {
        if (IsSensitive)
        {
            return Mask;
        }

        return Value.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Converts a <see cref="SingleValueObject{TSelf, TValue}"/> instance explicitly to its underlying value.
    /// </summary>
    /// <param name="valueObject">The value object to extract the value from.</param>
    /// <returns>The underlying wrapped value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="valueObject"/> is <see langword="null"/></exception>
    public static explicit operator TValue(SingleValueObject<TSelf, TValue> valueObject) =>
        valueObject is not null ? valueObject.Value : throw new ArgumentNullException(nameof(valueObject));
}

