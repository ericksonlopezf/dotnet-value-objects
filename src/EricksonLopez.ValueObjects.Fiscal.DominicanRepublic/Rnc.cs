// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents a Dominican Republic fiscal identifier: RNC (9 digits, Registro Nacional del Contribuyente)
/// or Cedula de Identidad (11 digits).
/// Validates format and DGII Mod-11 check digit.
///
/// <para><b>Rules:</b> Exactly 9 or 11 digits, valid Mod-11 check digit. Stored as digits-only.</para>
/// <para><b>Used by:</b> Residents, Expenses (Suppliers), Fiscal, Billing — Dominican Republic only.</para>
///
/// <para><b>⚠️ GEOGRAPHIC BOUNDARY:</b> This Value Object is <b>100% specific to the Dominican Republic</b>.
/// The Mod-11 check digit algorithm and 9/11-digit format are defined by the DGII
/// (Dirección General de Impuestos Internos). This VO MUST NOT be used for fiscal identifiers
/// from other jurisdictions (Mexico RFC, Spain NIF, Colombia NIT, USA EIN, etc.).</para>
///
/// <para>For multi-country deployments, each jurisdiction defines its own fiscal identifier VO
/// within its bounded context. The corporate <see cref="EricksonLopez.ValueObjects.NationalId"/>
/// provides a format-agnostic fallback when jurisdiction-specific validation is not required.</para>
/// </summary>
[RegulatoryRule("DO.RNC.001")]
public sealed record Rnc : StringValueObject<Rnc>
{
    private Rnc(string value) : base(value) { }

    /// <summary>
    /// Creates a validated <see cref="Rnc"/> instance from a raw or formatted 9-digit string.
    /// </summary>
    public static Result<Rnc> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Rnc>.Failure(Error.Validation(
                "Rnc.Required", "RNC is required."));
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

        if (digitCount != 9)
        {
            return Result<Rnc>.Failure(Error.Validation(
                "Rnc.InvalidLength", "RNC must contain exactly 9 numeric digits."));
        }

        ReadOnlySpan<char> digits = digitsBuffer[..9];
        if (!DgiiChecksum.ValidateRnc(digits))
        {
            return Result<Rnc>.Failure(Error.Validation(
                "Rnc.InvalidCheckDigit", $"RNC '{value}' has an invalid DGII Modulo 11 check digit."));
        }

        return Result<Rnc>.Success(new Rnc(new string(digits)));
    }

    /// <summary>
    /// Formats the RNC in the standard DGII format: <c>X-XX-XXXXX-X</c>.
    /// </summary>
    public string Formatted => $"{Value[0]}-{Value[1..3]}-{Value[3..8]}-{Value[8]}";
}



