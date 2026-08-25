// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an international postal or ZIP code.
/// </summary>
public sealed partial record PostalCode : StringValueObject<PostalCode>
{
    [GeneratedRegex(@"^[A-Z0-9][A-Z0-9 -]{1,18}[A-Z0-9]$")]
    private static partial Regex PostalCodePattern();

    private PostalCode(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="PostalCode"/> instance after normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw postal code string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated postal code, or a validation failure.</returns>
    public static Result<PostalCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(PostalCode), 3, 20,
            static n => new PostalCode(n), StringPipeline.NormalizeTrimUpper,
            PostalCodePattern(), "Postal code must contain letters, digits, spaces, or hyphens.");
    }
}


