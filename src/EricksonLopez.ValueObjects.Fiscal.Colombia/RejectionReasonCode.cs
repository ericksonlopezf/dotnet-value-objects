// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

/// <summary>
/// Represents an official DIAN rejection reason code for RADIAN Event 031 (Reclamo de la Factura Electrónica como Título Valor, Resolución 000085/2022).
///
/// <para><b>Official Codes:</b>
/// <list type="bullet">
///   <item><term>01</term><description>Documento con inconsistencias</description></item>
///   <item><term>02</term><description>Mercancía no entregada totalmente</description></item>
///   <item><term>03</term><description>Mercancía no entregada parcialmente</description></item>
///   <item><term>04</term><description>Servicio no prestado</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct RejectionReasonCode : ISpanParsable<RejectionReasonCode>, IEquatable<RejectionReasonCode>
{
    /// <summary>Gets rejection reason 01 (Documento con inconsistencias).</summary>
    public static RejectionReasonCode Inconsistencies => new("01", "Documento con inconsistencias");
    /// <summary>Gets rejection reason 02 (Mercancía no entregada totalmente).</summary>
    public static RejectionReasonCode GoodsNotDeliveredTotally => new("02", "Mercancía no entregada totalmente");
    /// <summary>Gets rejection reason 03 (Mercancía no entregada parcialmente).</summary>
    public static RejectionReasonCode GoodsNotDeliveredPartially => new("03", "Mercancía no entregada parcialmente");
    /// <summary>Gets rejection reason 04 (Servicio no prestado).</summary>
    public static RejectionReasonCode ServiceNotRendered => new("04", "Servicio no prestado");


    private readonly string _code;
    private readonly string _description;

    private RejectionReasonCode(string code, string description)
    {
        _code = code;
        _description = description;
    }

    /// <summary>
    /// Gets the 2-digit official DIAN rejection code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official DIAN description for the rejection reason.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// Creates a validated <see cref="RejectionReasonCode"/> from an official code ("01", "02", "03", "04").
    /// </summary>
    /// <param name="code">The 2-digit RADIAN rejection reason code.</param>
    /// <returns>A <see cref="Result{RejectionReasonCode}"/> containing the matched reason or a domain validation error.</returns>
    public static Result<RejectionReasonCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="RejectionReasonCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 2-digit rejection reason code.</param>
    /// <returns>A <see cref="Result{RejectionReasonCode}"/> containing the matched reason or a domain validation error.</returns>
    public static Result<RejectionReasonCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        return trimmed switch
        {
            "01" => Result<RejectionReasonCode>.Success(Inconsistencies),
            "02" => Result<RejectionReasonCode>.Success(GoodsNotDeliveredTotally),
            "03" => Result<RejectionReasonCode>.Success(GoodsNotDeliveredPartially),
            "04" => Result<RejectionReasonCode>.Success(ServiceNotRendered),
            _ => Result<RejectionReasonCode>.Failure(Error.Validation(
                "RejectionReasonCode.InvalidCode", $"The rejection reason code '{trimmed.ToString()}' is invalid for RADIAN (allowed: 01, 02, 03, 04)."))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public static RejectionReasonCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid RADIAN rejection reason code: '{s}'.");

    /// <inheritdoc/>
    public static RejectionReasonCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid RADIAN rejection reason code: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out RejectionReasonCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out RejectionReasonCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



