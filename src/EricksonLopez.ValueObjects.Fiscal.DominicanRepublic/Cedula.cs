// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents a validated Dominican Republic Cedula de Identidad y Electoral.
/// Exactly 11 numeric digits validated against the official Modulo 10 checksum algorithm.
/// </summary>
[RegulatoryRule("BASE.ID.006")]
public sealed record Cedula : StringValueObject<Cedula>
{
    private Cedula(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="Cedula"/> instance from a raw or formatted 11-digit string.
    /// </summary>
    /// <param name="value">The raw or formatted 11-digit Cedula string (hyphens and non-digit characters are stripped).</param>
    /// <returns>A <see cref="Result{Cedula}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Cedula> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Cedula>.Failure(Error.Validation(
                "Cedula.Required", "Cedula is required."));
        }

        Span<char> digitsBuffer = stackalloc char[value.Length];
        int digitCount = 0;
        foreach (char c in value)
        {
            if (char.IsAsciiDigit(c))
            {
                digitsBuffer[digitCount++] = c;
            }
        }

        if (digitCount != 11)
        {
            return Result<Cedula>.Failure(Error.Validation(
                "Cedula.InvalidLength", "Cedula must contain exactly 11 numeric digits."));
        }

        ReadOnlySpan<char> digits = digitsBuffer[..11];
        if (!CedulaChecksum.ValidateCedula(digits))
        {
            return Result<Cedula>.Failure(Error.Validation(
                "Cedula.InvalidCheckDigit", $"Cedula '{value}' has an invalid Modulo 10 check digit."));
        }

        return Result<Cedula>.Success(new Cedula(new string(digits)));
    }

    /// <summary>
    /// Formats the Cedula in the official format: <c>XXX-XXXXXXX-X</c>.
    /// </summary>
    public string Formatted => $"{Value[..3]}-{Value[3..10]}-{Value[10]}";
}


