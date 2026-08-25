// Copyright © Erickson Lopez. MIT License.
namespace EricksonLopez.ValueObjects;

/// <summary>
/// Provides a base record for composite value objects composed of multiple domain attributes.
/// </summary>
/// <remarks>
/// Inheriting records achieve structural value-based equality through C# record semantics.
/// Derived types should be sealed, maintain private constructors, and expose static factory methods returning <see cref="Result{T}"/>.
/// </remarks>
public abstract record ValueObject : DomainPrimitives.ValueObject, IValueObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValueObject"/> class.
    /// </summary>
    protected ValueObject() { }
}

