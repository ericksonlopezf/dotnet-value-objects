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
    /// Initializes a new instance of the <see cref="StringValueObject{TSelf}"/> class.
    /// </summary>
    /// <param name="value">The normalized string value to encapsulate.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/></exception>
    protected StringValueObject(string value) : base(value) { }
}


