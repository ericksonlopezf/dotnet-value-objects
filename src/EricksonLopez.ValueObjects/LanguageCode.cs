// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an ISO 639-1 or ISO 639-2 language code.
/// </summary>
public sealed partial record LanguageCode : StringValueObject<LanguageCode>
{
    [GeneratedRegex(@"^[a-z]{2,3}$")]
    private static partial Regex LanguagePattern();

    private LanguageCode(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="LanguageCode"/> instance after normalizing to lowercase.
    /// </summary>
    /// <param name="value">The ISO 639 language code string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated language code, or a validation failure.</returns>
    public static Result<LanguageCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(LanguageCode), 2, 3,
            static n => new LanguageCode(n), StringPipeline.NormalizeLower,
            LanguagePattern(),
            "Language code must be a 2 or 3 letter ISO 639 identifier.");
    }
}


