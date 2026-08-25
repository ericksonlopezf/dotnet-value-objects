// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents the 6-character alphanumeric Security Code (Código de Seguridad)
/// generated for an Electronic NCF (e-CF) to enable DGII QR-code and web validation.
/// </summary>
public sealed record SecurityCode : StringValueObject<SecurityCode>
{
    private SecurityCode(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="SecurityCode"/> from a 6-character alphanumeric string.
    /// </summary>
    /// <param name="value">The 6-character alphanumeric security code.</param>
    /// <returns>A <see cref="Result{SecurityCode}"/> containing the created instance or a validation error.</returns>
    public static Result<SecurityCode> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<SecurityCode>.Failure(Error.Validation(
                "SecurityCode.Required", "SecurityCode is required."));
        }

        string trimmed = value.Trim();
        if (trimmed.Length < 6)
        {
            return Result<SecurityCode>.Failure(Error.Validation(
                "SecurityCode.TooShort", "Security code must be at least 6 characters."));
        }

        if (trimmed.Length > 6)
        {
            return Result<SecurityCode>.Failure(Error.Validation(
                "SecurityCode.TooLong", "Security code cannot exceed 6 characters."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsAsciiLetterOrDigit(c))
            {
                return Result<SecurityCode>.Failure(Error.Validation(
                    "SecurityCode.InvalidFormat", "Security code must be exactly 6 alphanumeric characters."));
            }
        }

        return Result<SecurityCode>.Success(new SecurityCode(trimmed));
    }
}



