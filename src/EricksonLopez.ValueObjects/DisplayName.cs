// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a user-facing display name or alias for UI presentation.
/// </summary>
public sealed record DisplayName : StringValueObject<DisplayName>
{
    private DisplayName(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="DisplayName"/> instance after normalizing business whitespace.
    /// </summary>
    /// <param name="value">The raw display name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated display name, or a validation failure.</returns>
    public static Result<DisplayName> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(DisplayName), 2, 120,
            static n => new DisplayName(n),
            StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "Display name can contain letters, digits, spaces, and common business punctuation.");
    }
}


