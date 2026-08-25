// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Mexico;

using EricksonLopez.ValueObjects.Attributes;

/// <summary>
/// Represents an official SAT Payment Form Code (c_FormaPago, Anexo 20 CFDI 4.0).
///
/// <para><b>Common Codes:</b>
/// <list type="bullet">
///   <item><term>01</term><description>Efectivo</description></item>
///   <item><term>02</term><description>Cheque nominativo</description></item>
///   <item><term>03</term><description>Transferencia electrónica de fondos</description></item>
///   <item><term>04</term><description>Tarjeta de crédito</description></item>
///   <item><term>28</term><description>Tarjeta de débito</description></item>
///   <item><term>99</term><description>Por definir</description></item>
/// </list>
/// </para>
/// </summary>
[RegulatoryRule("CAT.VAL.002")]
[ValueObject]
public readonly record struct PaymentFormCode : ISpanParsable<PaymentFormCode>, IComparable<PaymentFormCode>
{
    /// <summary>Gets payment form 01 (Efectivo).</summary>
    public static PaymentFormCode Cash => new("01", "Efectivo", false);
    /// <summary>Gets payment form 02 (Cheque nominativo).</summary>
    public static PaymentFormCode Check => new("02", "Cheque nominativo", false);
    /// <summary>Gets payment form 03 (Transferencia electrónica de fondos).</summary>
    public static PaymentFormCode WireTransfer => new("03", "Transferencia electrónica de fondos", false);
    /// <summary>Gets payment form 04 (Tarjeta de crédito).</summary>
    public static PaymentFormCode CreditCard => new("04", "Tarjeta de crédito", false);
    /// <summary>Gets payment form 28 (Tarjeta de débito).</summary>
    public static PaymentFormCode DebitCard => new("28", "Tarjeta de débito", false);
    /// <summary>Gets payment form 99 (Por definir).</summary>
    public static PaymentFormCode ToBeDefined => new("99", "Por definir", true);


    private readonly string _code;
    private readonly string _description;
    private readonly bool _isDeferred;

    private PaymentFormCode(string code, string description, bool isDeferred)
    {
        _code = code;
        _description = description;
        _isDeferred = isDeferred;
    }

    /// <summary>
    /// Gets the 2-digit SAT payment form code.
    /// </summary>
    public string Code => _code;

    /// <summary>
    /// Gets the official description.
    /// </summary>
    public string Description => _description;

    /// <summary>Gets a value indicating whether this payment form represents deferred/pending definition (99).</summary>
    public bool IsDeferred => _isDeferred;

    /// <summary>
    /// Creates a validated <see cref="PaymentFormCode"/> from an official 2-digit code.
    /// </summary>
    /// <param name="code">The 2-digit SAT payment form code (e.g. <c>01</c>, <c>03</c>, <c>99</c>).</param>
    /// <returns>A <see cref="Result{PaymentFormCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PaymentFormCode> Create(string? code) =>
        Create(code.AsSpan());

    /// <summary>
    /// Creates a validated <see cref="PaymentFormCode"/> from a character span.
    /// </summary>
    /// <param name="input">A character span containing the 2-digit SAT payment form code.</param>
    /// <returns>A <see cref="Result{PaymentFormCode}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<PaymentFormCode> Create(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> trimmed = input.Trim();

        if (trimmed.Length != 2)
        {
            return Result<PaymentFormCode>.Failure(Error.Validation(
                "PaymentFormCode.InvalidLength", "The payment form code must contain exactly 2 characters."));
        }

        foreach (char c in trimmed)
        {
            if (!char.IsDigit(c))
            {
                return Result<PaymentFormCode>.Failure(Error.Validation(
                    "PaymentFormCode.InvalidCharacters", "The payment form code must only contain numeric digits."));
            }
        }

        return trimmed switch
        {
            "01" => Result<PaymentFormCode>.Success(Cash),
            "02" => Result<PaymentFormCode>.Success(Check),
            "03" => Result<PaymentFormCode>.Success(WireTransfer),
            "04" => Result<PaymentFormCode>.Success(CreditCard),
            "28" => Result<PaymentFormCode>.Success(DebitCard),
            "99" => Result<PaymentFormCode>.Success(ToBeDefined),
            _ => Result<PaymentFormCode>.Success(new PaymentFormCode(trimmed.ToString(), "Forma de Pago (Catálogo Dinámico)", false))
        };
    }

    /// <inheritdoc/>
    public override string ToString() => $"{_code} - {_description}";

    /// <inheritdoc/>
    public int CompareTo(PaymentFormCode other) => string.Compare(_code, other._code, StringComparison.Ordinal);

        /// <summary>
    /// Determines whether the left <see cref="PaymentFormCode"/> is less than the right <see cref="PaymentFormCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PaymentFormCode"/> to compare.</param>
    /// <param name="right">The second <see cref="PaymentFormCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <(PaymentFormCode left, PaymentFormCode right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether the left <see cref="PaymentFormCode"/> is less than or equal to the right <see cref="PaymentFormCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PaymentFormCode"/> to compare.</param>
    /// <param name="right">The second <see cref="PaymentFormCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator <=(PaymentFormCode left, PaymentFormCode right) => left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether the left <see cref="PaymentFormCode"/> is greater than the right <see cref="PaymentFormCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PaymentFormCode"/> to compare.</param>
    /// <param name="right">The second <see cref="PaymentFormCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >(PaymentFormCode left, PaymentFormCode right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether the left <see cref="PaymentFormCode"/> is greater than or equal to the right <see cref="PaymentFormCode"/>.
    /// </summary>
    /// <param name="left">The first <see cref="PaymentFormCode"/> to compare.</param>
    /// <param name="right">The second <see cref="PaymentFormCode"/> to compare.</param>
    /// <returns><see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
    public static bool operator >=(PaymentFormCode left, PaymentFormCode right) => left.CompareTo(right) >= 0;

    /// <inheritdoc/>
    public static PaymentFormCode Parse(string s, IFormatProvider? provider = null) =>
        TryParse(s.AsSpan(), provider, out var res) ? res : throw new FormatException($"Invalid PaymentFormCode: '{s}'.");

    /// <inheritdoc/>
    public static PaymentFormCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) =>
        TryParse(s, provider, out var res) ? res : throw new FormatException($"Invalid PaymentFormCode: '{s.ToString()}'.");

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out PaymentFormCode result)
    {
        var res = Create(s);
        result = res.IsSuccess ? res.Value : default;
        return res.IsSuccess;
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out PaymentFormCode result) =>
        TryParse(s.AsSpan(), provider, out result);
}



