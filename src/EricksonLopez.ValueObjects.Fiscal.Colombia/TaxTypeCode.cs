// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Colombia;

/// <summary>
/// Represents an official DIAN Tax Type Code (Tributos DIAN, Anexo Técnico 1.9).
///
/// <para><b>Official Codes:</b>
/// <list type="bullet">
///   <item><term>01</term><description>IVA (Impuesto sobre las Ventas)</description></item>
///   <item><term>02</term><description>INC (Impuesto Nacional al Consumo)</description></item>
///   <item><term>03</term><description>ICA (Impuesto de Industria y Comercio)</description></item>
///   <item><term>22</term><description>IBUA (Impuesto a las Bebidas Ultraprocesadas Azucaradas)</description></item>
///   <item><term>23</term><description>ICUI (Impuesto a los Productos Comestibles Ultraprocesados)</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct TaxTypeCode : ISpanParsable<TaxTypeCode>, IEquatable<TaxTypeCode>
{
    /// <summary>Gets tax type 01 (IVA - Impuesto sobre las Ventas).</summary>
    public static TaxTypeCode Iva => new("01", "IVA (Impuesto sobre las Ventas)");
    /// <summary>Gets tax type 02 (INC - Impuesto Nacional al Consumo).</summary>
    public static TaxTypeCode Inc => new("02", "INC (Impuesto Nacional al Consumo)");
    /// <summary>Gets tax type 03 (ICA - Impuesto de Industria y Comercio).</summary>
    public static TaxTypeCode Ica => new("03", "ICA (Impuesto de Industria y Comercio)");
    /// <summary>Gets tax type 22 (IBUA - Bebidas Ultraprocesadas Azucaradas).</summary>
    public static TaxTypeCode Ibua => new("22", "IBUA (Bebidas Ultraprocesadas Azucaradas)");
    /// <summary>Gets tax type 23 (ICUI - Comestibles Ultraprocesados).</summary>
    public static TaxTypeCode Icui => new("23", "ICUI (Comestibles Ultraprocesados)");


    private readonly string _code;
    private readonly string _description;

    private TaxTypeCode(string code, string description)
    {
        _code = code;
        _description = description;
    }

    /// <summary>
    /// Gets the official 2-digit DIAN tax code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official tax description.
    /// </summary>
    public string Description => _description;

    /// <summary>Gets a value indicating whether this tax represents IVA.</summary>
    public bool IsIva => _code == "01";

    /// <summary>Gets a value indicating whether this tax represents INC.</summary>
    public bool IsInc => _code == "02";

    /// <summary>Gets a value indicating whether this tax represents ICA.</summary>
    public bool IsIca => _code == "03";

    /// <summary>Gets a value indicating whether this tax represents IBUA.</summary>
    public bool IsIbua => _code == "22";

    /// <summary>Gets a value indicating whether this tax represents ICUI.</summary>
    public bool IsIcui => _code == "23";

    /// <summary>
    /// Creates a validated <see cref="TaxTypeCode"/> from an official 2-digit DIAN code.
    /// </summary>
    /// <param name="code">The 2-digit DIAN tax type code (e.g. <c>"01"</c> for IVA, <c>"03"</c> for ICA).</param>
    /// <returns>A <see cref="Result{TaxTypeCode}"/> containing the matched tax type or a domain validation error.</returns>
    public static Result<TaxTypeCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="TaxTypeCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 2-digit DIAN tax type code.</param>
    /// <returns>A <see cref="Result{TaxTypeCode}"/> containing the matched tax type or a domain validation error.</returns>
    public static Result<TaxTypeCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();
        return trimmed switch
        {
            "01" => Result<TaxTypeCode>.Success(Iva),
            "02" => Result<TaxTypeCode>.Success(Inc),
            "03" => Result<TaxTypeCode>.Success(Ica),
            "22" => Result<TaxTypeCode>.Success(Ibua),
            "23" => Result<TaxTypeCode>.Success(Icui),
            _ => Result<TaxTypeCode>.Failure(Error.Validation(
                "TaxTypeCode.InvalidCode", $"The DIAN tax code '{trimmed.ToString()}' is not recognized or valid."))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public static TaxTypeCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid DIAN tax code: '{s}'.");

    /// <inheritdoc/>
    public static TaxTypeCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid DIAN tax code: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TaxTypeCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out TaxTypeCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



