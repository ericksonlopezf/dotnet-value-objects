// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an ISO 3166-1 alpha-2 two-letter country code.
/// </summary>
public sealed partial record Country : StringValueObject<Country>
{
    [GeneratedRegex(@"^[A-Z]{2}$")]
    private static partial Regex CountryCodePattern();

    private Country(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="Country"/> instance from an ISO 3166-1 alpha-2 code string.
    /// </summary>
    /// <param name="value">The two-letter country code string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated country code, or a validation failure.</returns>
    public static Result<Country> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Country), 2, 2,
            static n => new Country(n), StringPipeline.NormalizeTrimUpper,
            CountryCodePattern(), "Country must be an ISO 3166-1 alpha-2 code (e.g., 'DO', 'US', 'ES').");
    }
}


