// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an individual's first or given name.
/// </summary>
public sealed record FirstName : StringValueObject<FirstName>
{
    private FirstName(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="FirstName"/> instance after normalizing whitespace and unicode characters.
    /// </summary>
    /// <param name="value">The raw first name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated first name, or a validation failure.</returns>
    public static Result<FirstName> Create(string? value)
    {
        return StringPipeline.Required(
            value,
            nameof(FirstName),
            1,
            80,
            static normalized => new FirstName(normalized),
            StringPipeline.NormalizeHumanName,
            StringPipeline.HumanNamePattern,
            "First name can contain letters, spaces, apostrophes, periods, or hyphens.");
    }
}


