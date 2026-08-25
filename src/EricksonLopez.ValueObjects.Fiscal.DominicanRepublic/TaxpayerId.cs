// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Smart polymorphic Value Object representing a Dominican Taxpayer Identifier (either an <see cref="Rnc"/> or a <see cref="Cedula"/>).
/// Automatically determines the taxpayer type by digit length and executes the corresponding DGII/JCE checksum validation.
/// </summary>
public sealed record TaxpayerId : ValueObject
{
    /// <summary>Gets the raw digits value.</summary>
    public string Value { get; }
    /// <summary>Gets the taxpayer identification type.</summary>
    public TaxpayerIdType Type { get; }
    /// <summary>Gets the formatted identifier (e.g. 1-01-00001-5 or 001-0000001-5).</summary>
    public string Formatted { get; }

    /// <summary>Gets the strongly-typed <see cref="Rnc"/> if this taxpayer is a corporate or business entity.</summary>
    public Rnc? AsRnc { get; }
    /// <summary>Gets the strongly-typed <see cref="Cedula"/> if this taxpayer is a personal national ID.</summary>
    public Cedula? AsCedula { get; }

    private TaxpayerId(string value, TaxpayerIdType type, string formatted, Rnc? rnc, Cedula? cedula)
    {
        Value = value;
        Type = type;
        Formatted = formatted;
        AsRnc = rnc;
        AsCedula = cedula;
    }

    /// <summary>
    /// Creates a <see cref="TaxpayerId"/> from a pre-validated <see cref="Rnc"/>.
    /// </summary>
    public static TaxpayerId FromRnc(Rnc rnc)
    {
        ArgumentNullException.ThrowIfNull(rnc, nameof(rnc));
        return new TaxpayerId(rnc.Value, TaxpayerIdType.Rnc, rnc.Formatted, rnc, null);
    }

    /// <summary>
    /// Creates a <see cref="TaxpayerId"/> from a pre-validated <see cref="Cedula"/>.
    /// </summary>
    public static TaxpayerId FromCedula(Cedula cedula)
    {
        ArgumentNullException.ThrowIfNull(cedula, nameof(cedula));
        return new TaxpayerId(cedula.Value, TaxpayerIdType.Cedula, cedula.Formatted, null, cedula);
    }

    /// <summary>
    /// Parses and validates a raw or formatted string as either an RNC (9 digits) or Cedula (11 digits).
    /// </summary>
    public static Result<TaxpayerId> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<TaxpayerId>.Failure(Error.Validation(
                "TaxpayerId.Required", "Taxpayer identifier (RNC or Cedula) is required."));
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

        if (digitCount == 9)
        {
            Result<Rnc> rncResult = Rnc.Create(new string(digitsBuffer[..9]));
            return rncResult.IsFailure
                ? Result<TaxpayerId>.Failure(rncResult.Error)
                : Result<TaxpayerId>.Success(FromRnc(rncResult.Value));
        }

        if (digitCount == 11)
        {
            Result<Cedula> cedulaResult = Cedula.Create(new string(digitsBuffer[..11]));
            return cedulaResult.IsFailure
                ? Result<TaxpayerId>.Failure(cedulaResult.Error)
                : Result<TaxpayerId>.Success(FromCedula(cedulaResult.Value));
        }

        return Result<TaxpayerId>.Failure(Error.Validation(
            "TaxpayerId.InvalidLength",
            $"Taxpayer identifier must have 9 digits (RNC) or 11 digits (Cedula). Found {digitCount} digits in '{value}'."));
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}


