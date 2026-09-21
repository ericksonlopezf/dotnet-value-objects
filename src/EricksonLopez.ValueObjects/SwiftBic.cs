// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a Business Identifier Code (SWIFT/BIC) compliant with ISO 9362.
/// Formats: 8 characters (primary/head office) or 11 characters (specific branch).
/// </summary>
public sealed record SwiftBic : StringValueObject<SwiftBic>
{
    private SwiftBic(string value) : base(value) { }

    /// <summary>Gets the 4-letter bank identifier code.</summary>
    public string BankCode => Value[..4];

    /// <summary>Gets the 2-letter ISO 3166-1 alpha-2 country code.</summary>
    public string CountryCode => Value[4..6];

    /// <summary>Gets the 2-character location code.</summary>
    public string LocationCode => Value[6..8];

    /// <summary>Gets the 3-character branch code, or <see langword="null"/> if this is an 8-character head office code.</summary>
    public string? BranchCode => Value.Length == 11 ? Value[8..11] : null;

    /// <summary>Gets a value indicating whether this BIC represents a primary or head office (8 characters).</summary>
    public bool IsHeadOffice => Value.Length == 8;

    /// <summary>Reconstitutes an instance from persistence storage without validation.</summary>
    internal static SwiftBic Reconstitute(string value) => new(value);

    /// <summary>Hydrates an instance from persistence storage.</summary>
    internal static SwiftBic Hydrate(string value) => new(value);

    /// <summary>
    /// Creates a validated <see cref="SwiftBic"/> instance conforming to ISO 9362 specifications.
    /// </summary>
    /// <param name="value">The raw 8- or 11-character SWIFT/BIC string.</param>
    /// <returns>A <see cref="Result{SwiftBic}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<SwiftBic> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<SwiftBic>.Failure(Error.Validation(
                "SwiftBic.Required", "SWIFT/BIC code is required."));
        }

        string normalized = value.Trim().ToUpperInvariant();
        if (normalized.Length is not (8 or 11))
        {
            return Result<SwiftBic>.Failure(Error.Validation(
                "SwiftBic.InvalidLength", "SWIFT/BIC code must be exactly 8 or 11 characters long."));
        }

        // Bank code (positions 0-3) must be alphabetic
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsAsciiLetter(normalized[i]))
            {
                return Result<SwiftBic>.Failure(Error.Validation(
                    "SwiftBic.InvalidBankCode", "SWIFT/BIC bank code (first 4 characters) must be letters."));
            }
        }

        // Country code (positions 4-5) must be alphabetic
        for (int i = 4; i < 6; i++)
        {
            if (!char.IsAsciiLetter(normalized[i]))
            {
                return Result<SwiftBic>.Failure(Error.Validation(
                    "SwiftBic.InvalidCountryCode", "SWIFT/BIC country code (positions 5-6) must be letters."));
            }
        }

        // Location code (positions 6-7) must be alphanumeric
        for (int i = 6; i < 8; i++)
        {
            if (!char.IsAsciiLetterOrDigit(normalized[i]))
            {
                return Result<SwiftBic>.Failure(Error.Validation(
                    "SwiftBic.InvalidLocationCode", "SWIFT/BIC location code (positions 7-8) must be alphanumeric."));
            }
        }

        // Branch code (positions 8-10, if 11 chars) must be alphanumeric
        if (normalized.Length == 11)
        {
            for (int i = 8; i < 11; i++)
            {
                if (!char.IsAsciiLetterOrDigit(normalized[i]))
                {
                    return Result<SwiftBic>.Failure(Error.Validation(
                        "SwiftBic.InvalidBranchCode", "SWIFT/BIC branch code (positions 9-11) must be alphanumeric."));
                }
            }
        }

        return Result<SwiftBic>.Success(new SwiftBic(normalized));
    }
}
