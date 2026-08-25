// Copyright © Erickson Lopez. MIT License.
using System;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Marks a value object, class, or property as containing sensitive data requiring redaction or masking.
/// </summary>
/// <remarks>
/// Governs formatting behavior to prevent accidental leakage in diagnostic logs and traces.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class SensitiveDataAttribute : Attribute
{
    /// <summary>
    /// Gets the mask pattern used when displaying sensitive values.
    /// </summary>
    public string Mask { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SensitiveDataAttribute"/> class with an optional mask pattern.
    /// </summary>
    /// <param name="mask">The mask string to display in place of the sensitive value. Defaults to <c>"***"</c>.</param>
    public SensitiveDataAttribute(string mask = "***")
    {
        Mask = mask;
    }
}
