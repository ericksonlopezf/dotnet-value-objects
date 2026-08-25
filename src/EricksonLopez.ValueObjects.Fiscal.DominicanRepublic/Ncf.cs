// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents a traditional Dominican Republic NCF (Número de Comprobante Fiscal) Serie B.
/// Format: 11 characters: <c>B</c> + 2 digits type + 8 digits sequential number (e.g. <c>B0100000001</c>).
/// </summary>
public sealed record Ncf : StringValueObject<Ncf>
{
    /// <summary>Gets the traditional NCF series prefix ('B').</summary>
    public const char Series = 'B';

    /// <summary>Gets the traditional NCF type.</summary>
    public NcfType Type { get; }
    /// <summary>Gets the 8-digit sequential number.</summary>
    public int Sequence { get; }

    private Ncf(string value, NcfType type, int sequence) : base(value)
    {
        Type = type;
        Sequence = sequence;
    }

    /// <summary>
    /// Creates an <see cref="Ncf"/> from a raw 11-character string (e.g. <c>B0100000001</c>).
    /// </summary>
    /// <param name="value">The raw 11-character traditional NCF string.</param>
    /// <returns>A <see cref="Result{Ncf}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Ncf> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Ncf>.Failure(Error.Validation(
                "Ncf.Required", "NCF is required."));
        }

        string trimmed = value.Trim();
        if (trimmed.Length != 11 || (trimmed[0] != 'B' && trimmed[0] != 'b'))
        {
            return Result<Ncf>.Failure(Error.Validation(
                "Ncf.InvalidFormat",
                $"NCF '{value}' is invalid. Format must be 'B' followed by a 2-digit type (01, 02, 03, 04, 11-17) and an 8-digit sequence (11 chars total)."));
        }

        string typeCode = trimmed[1..3];
        Result<NcfType> typeResult = NcfType.Create(typeCode);
        if (typeResult.IsFailure)
        {
            return Result<Ncf>.Failure(Error.Validation(
                "Ncf.InvalidFormat",
                $"NCF '{value}' is invalid. Format must be 'B' followed by a 2-digit type (01, 02, 03, 04, 11-17) and an 8-digit sequence (11 chars total)."));
        }

        ReadOnlySpan<char> seqSpan = trimmed.AsSpan(3, 8);
        foreach (char c in seqSpan)
        {
            if (!char.IsAsciiDigit(c))
            {
                return Result<Ncf>.Failure(Error.Validation(
                    "Ncf.InvalidFormat",
                    $"NCF '{value}' is invalid. Format must be 'B' followed by a 2-digit type (01, 02, 03, 04, 11-17) and an 8-digit sequence (11 chars total)."));
            }
        }

        int sequence = int.Parse(seqSpan, CultureInfo.InvariantCulture);
        if (sequence <= 0)
        {
            return Result<Ncf>.Failure(Error.Validation(
                "Ncf.InvalidSequence", "NCF sequence must be greater than zero."));
        }

        string normalized = $"B{typeCode}{sequence.ToString("D8", CultureInfo.InvariantCulture)}";
        return Result<Ncf>.Success(new Ncf(normalized, typeResult.Value, sequence));
    }


    /// <summary>
    /// Creates an <see cref="Ncf"/> from a strongly-typed <see cref="NcfType"/> and integer sequence.
    /// </summary>
    /// <param name="type">The traditional NCF document type.</param>
    /// <param name="sequence">The sequential number between 1 and 99,999,999.</param>
    /// <returns>A <see cref="Result{Ncf}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<Ncf> Create(NcfType type, int sequence)
    {
        if (sequence is <= 0 or > 99_999_999)
        {
            return Result<Ncf>.Failure(Error.Validation(
                "Ncf.SequenceOutOfRange",
                $"NCF sequence must be between 1 and 99,999,999. Given: {sequence}."));
        }

        string formatted = $"B{type.Code}{sequence.ToString("D8", CultureInfo.InvariantCulture)}";
        return Result<Ncf>.Success(new Ncf(formatted, type, sequence));
    }
}

