// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an individual's last name or surname.
/// </summary>
public sealed record LastName : StringValueObject<LastName>
{
    private LastName(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="LastName"/> instance after normalizing whitespace and unicode characters.
    /// </summary>
    /// <param name="value">The raw last name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated last name, or a validation failure.</returns>
    public static Result<LastName> Create(string? value)
    {
        return StringPipeline.Required(
            value,
            nameof(LastName),
            1,
            120,
            static normalized => new LastName(normalized),
            StringPipeline.NormalizeHumanName,
            StringPipeline.HumanNamePattern,
            "Last name can contain letters, spaces, apostrophes, periods, or hyphens.");
    }
}


