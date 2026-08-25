// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using EricksonLopez.Result;
using EricksonLopez.ValueObjects.Attributes;

namespace EricksonLopez.ValueObjects.Fiscal.DominicanRepublic;

/// <summary>
/// Represents a Dominican Republic Electronic NCF (e-CF) Serie E governed by Law 32-23.
/// Format: 13 characters: <c>E</c> + 2 digits type + 10 digits sequential number (e.g. <c>E310000000001</c>).
/// </summary>
[RegulatoryRule("DOC.SEQ.001")]
public sealed record ElectronicNcf : StringValueObject<ElectronicNcf>
{
    /// <summary>Gets the electronic NCF series prefix ('E').</summary>
    public const char Series = 'E';

    /// <summary>Gets the electronic NCF type.</summary>
    public EcfType Type { get; }
    /// <summary>Gets the 10-digit sequential number.</summary>
    public long Sequence { get; }
    /// <summary>Gets the associated DGII security code, if present.</summary>
    public SecurityCode? SecurityCode { get; }

    private ElectronicNcf(string value, EcfType type, long sequence, SecurityCode? securityCode) : base(value)
    {
        Type = type;
        Sequence = sequence;
        SecurityCode = securityCode;
    }

    /// <summary>
    /// Creates an <see cref="ElectronicNcf"/> from a raw 13-character string (e.g. <c>E310000000001</c>).
    /// </summary>
    /// <param name="value">The raw 13-character electronic NCF string.</param>
    /// <param name="securityCode">The optional DGII security code to associate with the NCF.</param>
    /// <returns>A <see cref="Result{ElectronicNcf}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<ElectronicNcf> Create(string? value, SecurityCode? securityCode = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<ElectronicNcf>.Failure(Error.Validation(
                "ElectronicNcf.Required", "Electronic NCF (e-CF) is required."));
        }

        string trimmed = value.Trim();
        if (trimmed.Length != 13 || (trimmed[0] != 'E' && trimmed[0] != 'e'))
        {
            return Result<ElectronicNcf>.Failure(Error.Validation(
                "ElectronicNcf.InvalidFormat",
                $"Electronic NCF '{value}' is invalid. Format must be 'E' followed by a 2-digit type (31-34, 41, 43-47) and a 10-digit sequence (13 chars total)."));
        }

        string typeCode = trimmed[1..3];
        Result<EcfType> typeResult = EcfType.Create(typeCode);
        if (typeResult.IsFailure)
        {
            return Result<ElectronicNcf>.Failure(Error.Validation(
                "ElectronicNcf.InvalidFormat",
                $"Electronic NCF '{value}' is invalid. Format must be 'E' followed by a 2-digit type (31-34, 41, 43-47) and a 10-digit sequence (13 chars total)."));
        }

        ReadOnlySpan<char> seqSpan = trimmed.AsSpan(3, 10);
        foreach (char c in seqSpan)
        {
            if (!char.IsAsciiDigit(c))
            {
                return Result<ElectronicNcf>.Failure(Error.Validation(
                    "ElectronicNcf.InvalidFormat",
                    $"Electronic NCF '{value}' is invalid. Format must be 'E' followed by a 2-digit type (31-34, 41, 43-47) and a 10-digit sequence (13 chars total)."));
            }
        }

        long sequence = long.Parse(seqSpan, CultureInfo.InvariantCulture);
        if (sequence <= 0)
        {
            return Result<ElectronicNcf>.Failure(Error.Validation(
                "ElectronicNcf.InvalidSequence", "e-CF sequence must be greater than zero."));
        }

        string normalized = $"E{typeCode}{sequence.ToString("D10", CultureInfo.InvariantCulture)}";
        return Result<ElectronicNcf>.Success(new ElectronicNcf(normalized, typeResult.Value, sequence, securityCode));
    }


    /// <summary>
    /// Creates an <see cref="ElectronicNcf"/> from a strongly-typed <see cref="EcfType"/> and 10-digit sequential number.
    /// </summary>
    /// <param name="type">The electronic NCF document type.</param>
    /// <param name="sequence">The sequential number between 1 and 9,999,999,999.</param>
    /// <param name="securityCode">The optional DGII security code to associate with the NCF.</param>
    /// <returns>A <see cref="Result{ElectronicNcf}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<ElectronicNcf> Create(EcfType type, long sequence, SecurityCode? securityCode = null)
    {
        if (sequence is <= 0 or > 9_999_999_999L)
        {
            return Result<ElectronicNcf>.Failure(Error.Validation(
                "ElectronicNcf.SequenceOutOfRange",
                $"e-CF sequence must be between 1 and 9,999,999,999. Given: {sequence}."));
        }

        string formatted = $"E{type.Code}{sequence.ToString("D10", CultureInfo.InvariantCulture)}";
        return Result<ElectronicNcf>.Success(new ElectronicNcf(formatted, type, sequence, securityCode));
    }

    /// <summary>
    /// Returns a new instance associated with the provided DGII <see cref="SecurityCode"/>.
    /// </summary>
    /// <param name="securityCode">The DGII security code to associate.</param>
    /// <returns>A new <see cref="ElectronicNcf"/> instance with the specified security code.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="securityCode"/> is <see langword="null"/></exception>
    public ElectronicNcf WithSecurityCode(SecurityCode securityCode)
    {
        ArgumentNullException.ThrowIfNull(securityCode, nameof(securityCode));
        return new ElectronicNcf(Value, Type, Sequence, securityCode);
    }
}

