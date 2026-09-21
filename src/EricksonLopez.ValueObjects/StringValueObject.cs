// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides a base record for value objects whose encapsulated state is a single normalized string.
/// </summary>
/// <remarks>
/// Eliminates boilerplate by inheriting value equality, comparison semantics, and string representation from <see cref="SingleValueObject{TSelf, TValue}"/>.
/// </remarks>
/// <typeparam name="TSelf">
/// The concrete string value object type deriving from this base.
/// <para>
/// This type parameter is annotated with
/// <see cref="System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All"/> to ensure full
/// Native AOT and trimmer compatibility. Deriving types must be concrete, non-abstract records.
/// </para>
/// </typeparam>
public abstract record StringValueObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TSelf> : SingleValueObject<TSelf, string>
    where TSelf : StringValueObject<TSelf>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StringValueObject{TSelf}"/> class, 
    /// explicitly normalizing the string to Unicode FormC to prevent canonicalization vulnerabilities,
    /// and enforcing a hard 20,000 character maximum limit as a last-resort guard against Large Object Heap (LOH) exhaustion.
    /// </summary>
    /// <remarks>
    /// Each concrete value object is expected to enforce its own, stricter domain-specific maximum via its
    /// factory method returning <see cref="EricksonLopez.Result.Result{T}"/>. This base limit is a safety net only.
    /// A 20,000-character UTF-16 string is approximately 40 KB, well below the 85 KB LOH threshold.
    /// </remarks>
    /// <param name="value">The raw string value to encapsulate and normalize.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentException">The string exceeds the maximum allowed length of 20,000 characters.</exception>
    protected StringValueObject(string value) : base(
        (value?.Length > 20000 ? throw new ArgumentException("Value exceeds the maximum allowed length of 20,000 characters to prevent memory exhaustion.", nameof(value)) : value)?
        .Normalize(System.Text.NormalizationForm.FormC)!)
    { }
}


