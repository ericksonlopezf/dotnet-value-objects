// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a generic business or entity name identifier.
/// </summary>
public sealed record Name : StringValueObject<Name>
{
    private Name(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="Name"/> instance after normalizing business whitespace.
    /// </summary>
    /// <param name="value">The raw name string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated name, or a validation failure.</returns>
    public static Result<Name> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Name), 1, 200,
            static n => new Name(n),
            StringPipeline.NormalizeBusinessName,
            StringPipeline.BusinessNamePattern,
            "Name can contain letters, digits, spaces, and common business punctuation.");
    }
}


