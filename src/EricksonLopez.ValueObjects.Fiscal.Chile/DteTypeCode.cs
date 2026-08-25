// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

/// <summary>
/// Represents an official Chilean DTE Type Code (Tipo de Documento Tributario Electrónico)
/// recognized by the SII (Resolución Exenta N° 45/2003 y modificatorias).
///
/// <para><b>Official Codes:</b>
/// <list type="bullet">
///   <item><term>33</term><description>Factura Electrónica</description></item>
///   <item><term>34</term><description>Factura No Afecta o Exenta Electrónica</description></item>
///   <item><term>39</term><description>Boleta Electrónica</description></item>
///   <item><term>41</term><description>Boleta Exenta Electrónica</description></item>
///   <item><term>46</term><description>Factura de Compra Electrónica</description></item>
///   <item><term>52</term><description>Guía de Despacho Electrónica</description></item>
///   <item><term>56</term><description>Nota de Débito Electrónica</description></item>
///   <item><term>61</term><description>Nota de Crédito Electrónica</description></item>
///   <item><term>110</term><description>Factura de Exportación Electrónica</description></item>
///   <item><term>111</term><description>Nota de Débito de Exportación Electrónica</description></item>
///   <item><term>112</term><description>Nota de Crédito de Exportación Electrónica</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct DteTypeCode : ISpanParsable<DteTypeCode>, IComparable<DteTypeCode>
{
    /// <summary>Gets DTE code 33 (Factura Electrónica).</summary>
    public static DteTypeCode FacturaElectronica => new(33, "Factura Electrónica");
    /// <summary>Gets DTE code 34 (Factura No Afecta o Exenta Electrónica).</summary>
    public static DteTypeCode FacturaExenta => new(34, "Factura No Afecta o Exenta Electrónica");
    /// <summary>Gets DTE code 39 (Boleta Electrónica).</summary>
    public static DteTypeCode BoletaElectronica => new(39, "Boleta Electrónica");
    /// <summary>Gets DTE code 41 (Boleta Exenta Electrónica).</summary>
    public static DteTypeCode BoletaExenta => new(41, "Boleta Exenta Electrónica");
    /// <summary>Gets DTE code 46 (Factura de Compra Electrónica).</summary>
    public static DteTypeCode FacturaCompra => new(46, "Factura de Compra Electrónica");
    /// <summary>Gets DTE code 52 (Guía de Despacho Electrónica).</summary>
    public static DteTypeCode GuiaDespacho => new(52, "Guía de Despacho Electrónica");
    /// <summary>Gets DTE code 56 (Nota de Débito Electrónica).</summary>
    public static DteTypeCode NotaDebito => new(56, "Nota de Débito Electrónica");
    /// <summary>Gets DTE code 61 (Nota de Crédito Electrónica).</summary>
    public static DteTypeCode NotaCredito => new(61, "Nota de Crédito Electrónica");
    /// <summary>Gets DTE code 110 (Factura de Exportación Electrónica).</summary>
    public static DteTypeCode FacturaExportacion => new(110, "Factura de Exportación Electrónica");
    /// <summary>Gets DTE code 111 (Nota de Débito de Exportación Electrónica).</summary>
    public static DteTypeCode NotaDebitoExportacion => new(111, "Nota de Débito de Exportación Electrónica");
    /// <summary>Gets DTE code 112 (Nota de Crédito de Exportación Electrónica).</summary>
    public static DteTypeCode NotaCreditoExportacion => new(112, "Nota de Crédito de Exportación Electrónica");


    private readonly int _code;
    private readonly string _name;

    private DteTypeCode(int code, string name)
    {
        _code = code;
        _name = name;
    }

    /// <summary>
    /// Gets the integer DTE code (e.g. 33).
    /// </summary>
    public int Code => _code;

    /// <summary>
    /// Gets the official name of the DTE.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Creates a validated <see cref="DteTypeCode"/> from an integer code.
    /// </summary>
    /// <param name="code">The integer SII DTE type code (e.g. 33, 34, 39).</param>
    /// <returns>A <see cref="Result{DteTypeCode}"/> containing the matched type or a domain validation error.</returns>
    public static Result<DteTypeCode> Create(int code)
    {
        return code switch
        {
            33 => Result<DteTypeCode>.Success(FacturaElectronica),
            34 => Result<DteTypeCode>.Success(FacturaExenta),
            39 => Result<DteTypeCode>.Success(BoletaElectronica),
            41 => Result<DteTypeCode>.Success(BoletaExenta),
            46 => Result<DteTypeCode>.Success(FacturaCompra),
            52 => Result<DteTypeCode>.Success(GuiaDespacho),
            56 => Result<DteTypeCode>.Success(NotaDebito),
            61 => Result<DteTypeCode>.Success(NotaCredito),
            110 => Result<DteTypeCode>.Success(FacturaExportacion),
            111 => Result<DteTypeCode>.Success(NotaDebitoExportacion),
            112 => Result<DteTypeCode>.Success(NotaCreditoExportacion),
            _ => Result<DteTypeCode>.Failure(Error.Validation(
                "DteTypeCode.InvalidCode", $"The DTE code '{code.ToString(CultureInfo.InvariantCulture)}' is not recognized by the SII."))
        };
    }

    /// <summary>
    /// Creates a validated <see cref="DteTypeCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the numeric DTE type code.</param>
    /// <returns>A <see cref="Result{DteTypeCode}"/> containing the matched type or a domain validation error.</returns>
    public static Result<DteTypeCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        if (!int.TryParse(trimmed, CultureInfo.InvariantCulture, out int code))
        {
            return Result<DteTypeCode>.Failure(Error.Validation(
                "DteTypeCode.InvalidFormat", "The DTE code must be numeric."));
        }

        return Create(code);
    }

    /// <summary>
    /// Creates a validated <see cref="DteTypeCode"/> from a nullable string.
    /// </summary>
    /// <param name="input">A string containing the numeric DTE type code.</param>
    /// <returns>A <see cref="Result{DteTypeCode}"/> containing the matched type or a domain validation error.</returns>
    public static Result<DteTypeCode> Create(string? input) =>
        Create(input.AsSpan());

    /// <inheritdoc/>
    public override string ToString() => $"{_code.ToString(CultureInfo.InvariantCulture)} - {_name}";

    /// <inheritdoc/>
    public int CompareTo(DteTypeCode other) => _code.CompareTo(other._code);

        /// <summary>
    /// Determines whether the left <see cref="DteTypeCode"/> is less than the right <see cref="DteTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DteTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="DteTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(DteTypeCode left, DteTypeCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="DteTypeCode"/> is less than or equal to the right <see cref="DteTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DteTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="DteTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(DteTypeCode left, DteTypeCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="DteTypeCode"/> is greater than the right <see cref="DteTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DteTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="DteTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(DteTypeCode left, DteTypeCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="DteTypeCode"/> is greater than or equal to the right <see cref="DteTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="DteTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="DteTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(DteTypeCode left, DteTypeCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static DteTypeCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid DteTypeCode: '{s}'.");

    /// <inheritdoc/>
    public static DteTypeCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid DteTypeCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out DteTypeCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out DteTypeCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}




