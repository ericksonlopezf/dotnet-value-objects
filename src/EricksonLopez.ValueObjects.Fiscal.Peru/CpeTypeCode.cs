// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Peru;

/// <summary>
/// Represents an official SUNAT CPE Document Type Code (Tipo de Comprobante de Pago Electrónico, Catálogo N° 01).
///
/// <para><b>Official Codes:</b>
/// <list type="bullet">
///   <item><term>01</term><description>Factura</description></item>
///   <item><term>03</term><description>Boleta de Venta</description></item>
///   <item><term>07</term><description>Nota de Crédito</description></item>
///   <item><term>08</term><description>Nota de Débito</description></item>
///   <item><term>09</term><description>Guía de Remisión Remitente</description></item>
///   <item><term>31</term><description>Guía de Remisión Transportista</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct CpeTypeCode : ISpanParsable<CpeTypeCode>, IComparable<CpeTypeCode>
{
    /// <summary>Gets CPE code 01 (Factura).</summary>
    public static CpeTypeCode Factura => new("01", "Factura");
    /// <summary>Gets CPE code 03 (Boleta de Venta).</summary>
    public static CpeTypeCode Boleta => new("03", "Boleta de Venta");
    /// <summary>Gets CPE code 07 (Nota de Crédito).</summary>
    public static CpeTypeCode NotaCredito => new("07", "Nota de Crédito");
    /// <summary>Gets CPE code 08 (Nota de Débito).</summary>
    public static CpeTypeCode NotaDebito => new("08", "Nota de Débito");
    /// <summary>Gets CPE code 09 (Guía de Remisión Remitente).</summary>
    public static CpeTypeCode GuiaRemitente => new("09", "Guía de Remisión Remitente");
    /// <summary>Gets CPE code 31 (Guía de Remisión Transportista).</summary>
    public static CpeTypeCode GuiaTransportista => new("31", "Guía de Remisión Transportista");


    private readonly string _code;
    private readonly string _name;

    private CpeTypeCode(string code, string name)
    {
        _code = code;
        _name = name;
    }

    /// <summary>
    /// Gets the 2-digit SUNAT code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official document name.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Creates a validated <see cref="CpeTypeCode"/> from a 2-digit string.
    /// </summary>
    public static Result<CpeTypeCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="CpeTypeCode"/> from a character span.
    /// </summary>
    public static Result<CpeTypeCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        return trimmed switch
        {
            "01" => Result<CpeTypeCode>.Success(Factura),
            "03" => Result<CpeTypeCode>.Success(Boleta),
            "07" => Result<CpeTypeCode>.Success(NotaCredito),
            "08" => Result<CpeTypeCode>.Success(NotaDebito),
            "09" => Result<CpeTypeCode>.Success(GuiaRemitente),
            "31" => Result<CpeTypeCode>.Success(GuiaTransportista),
            _ => Result<CpeTypeCode>.Failure(Error.Validation(
                "CpeTypeCode.InvalidCode", $"The CPE document type '{trimmed.ToString()}' is not recognized by SUNAT."))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_name}";

    /// <inheritdoc/>
    public int CompareTo(CpeTypeCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="CpeTypeCode"/> is less than the right <see cref="CpeTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(CpeTypeCode left, CpeTypeCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeTypeCode"/> is less than or equal to the right <see cref="CpeTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(CpeTypeCode left, CpeTypeCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeTypeCode"/> is greater than the right <see cref="CpeTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(CpeTypeCode left, CpeTypeCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="CpeTypeCode"/> is greater than or equal to the right <see cref="CpeTypeCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="CpeTypeCode"/> to compare.</param>
    /// <param name="right">The second <see cref="CpeTypeCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(CpeTypeCode left, CpeTypeCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static CpeTypeCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid CpeTypeCode: '{s}'.");

    /// <inheritdoc/>
    public static CpeTypeCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid CpeTypeCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out CpeTypeCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out CpeTypeCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



