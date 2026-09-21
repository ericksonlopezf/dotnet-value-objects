// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents a validated Dominican Republic Social Security Number (NSS - Número de Seguridad Social, Tesorería de la Seguridad Social).
/// Exactly 9 numeric digits validated against the official TSS Modulo 10 check-digit algorithm.
/// </summary>
[RegulatoryRule("DO.TSS.001")]
public sealed record Nss : StringValueObject<Nss>
{
    private Nss(string value) : base(value) { }

    /// <summary>Reconstitutes an instance from persistence storage without validation.</summary>
    internal static Nss Reconstitute(string value) => new(value);

    /// <summary>Hydrates an instance from persistence storage.</summary>
    internal static Nss Hydrate(string value) => new(value);

    /// <summary>
    /// Creates a validated <see cref="Nss"/> instance from a raw or formatted 9-digit string.
    /// </summary>
    /// <param name="value">The raw or formatted 9-digit NSS string (hyphens and non-digit characters are stripped).</param>
    /// <returns>A <see cref="Result{Nss}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Nss> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Nss>.Failure(Error.Validation(
                "Nss.Required", "NSS is required."));
        }

        if (value.Length > 32)
        {
            return Result<Nss>.Failure(Error.Validation(
                "Nss.InvalidLength", "NSS input exceeds maximum allowed length."));
        }

        Span<char> digitsBuffer = stackalloc char[32];
        int digitCount = 0;
        foreach (char c in value)
        {
            if (char.IsAsciiDigit(c))
            {
                digitsBuffer[digitCount++] = c;
            }
        }

        if (digitCount != 9)
        {
            return Result<Nss>.Failure(Error.Validation(
                "Nss.InvalidLength", "NSS must contain exactly 9 numeric digits."));
        }

        ReadOnlySpan<char> digits = digitsBuffer[..9];
        if (!NssChecksum.ValidateNss(digits))
        {
            return Result<Nss>.Failure(Error.Validation(
                "Nss.InvalidCheckDigit", $"NSS '{value}' has an invalid TSS Modulo 10 check digit."));
        }

        return Result<Nss>.Success(new Nss(new string(digits)));
    }

    /// <summary>
    /// Formats the NSS in the standard format: <c>XXX-XXXXXX</c>.
    /// </summary>
    public string Formatted => $"{Value[..3]}-{Value[3..]}";
}
