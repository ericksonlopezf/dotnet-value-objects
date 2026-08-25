// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Instructs the source generator to synthesize factories, parsers, formatters, and equality for this value object.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class ValueObjectAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a value indicating whether to generate implicit and explicit conversion operators to and from the underlying value.
    /// </summary>
    public bool GenerateConversionOperators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to generate persistence type handler hooks.
    /// </summary>
    public bool GeneratePersistenceHooks { get; set; } = true;
}
