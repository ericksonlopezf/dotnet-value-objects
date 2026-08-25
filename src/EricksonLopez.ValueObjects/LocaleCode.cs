// Copyright © Erickson Lopez. MIT License.
using System;
using System.Linq;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a locale or culture code identifier.
/// </summary>
public sealed partial record LocaleCode : StringValueObject<LocaleCode>
{
    [GeneratedRegex(@"^[a-z]{2,3}(-[A-Z0-9]{2,4})?$")]
    private static partial Regex LocalePattern();

    private LocaleCode(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="LocaleCode"/> instance after normalizing language and region casing.
    /// </summary>
    /// <param name="value">The locale identifier string.</param>
    /// <returns>A successful <see cref="Result{T}"/> containing the validated locale code, or a validation failure.</returns>
    public static Result<LocaleCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(LocaleCode), 2, 10,
            static n => new LocaleCode(n), NormalizeLocale,
            LocalePattern(),
            "Locale code must be formatted as 'language' or 'language-COUNTRY' (e.g., 'es-DO', 'en-US').");
    }

    private static string NormalizeLocale(string raw)
    {
        string trimmed = raw.Trim().Replace('_', '-');
        ReadOnlySpan<char> span = trimmed.AsSpan();
        int dashIndex = span.IndexOf('-');
        if (dashIndex == -1)
        {
            return trimmed.ToLowerInvariant();
        }

        return string.Concat(
            trimmed.AsSpan(0, dashIndex).ToString().ToLowerInvariant(),
            "-",
            trimmed.AsSpan(dashIndex + 1).ToString().ToUpperInvariant());
    }
}



