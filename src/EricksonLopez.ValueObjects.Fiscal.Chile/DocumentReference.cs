// Copyright © Erickson Lopez. MIT License.
using System;
using System.Globalization;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects.Fiscal.Chile;

/// <summary>
/// Represents a formal Chilean DTE Document Reference (Referencia DTE, Ley 19.983 / SII)
/// used in Credit/Debit Notes to reference the original document.
///
/// <para><b>Reference Codes (<see cref="ReferenceCode"/>):</b>
/// <list type="bullet">
///   <item><term>1</term><description>Anula Documento de Referencia</description></item>
///   <item><term>2</term><description>Corrige Texto Documento de Referencia</description></item>
///   <item><term>3</term><description>Corrige Montos</description></item>
/// </list>
/// </para>
/// </summary>
[ValueObject]
public readonly record struct DocumentReference : IEquatable<DocumentReference>
{
    /// <summary>Gets the target DTE document type code.</summary>
    public DteTypeCode TargetType { get; }
    /// <summary>Gets the fiscal folio number of the referenced document.</summary>
    public FiscalFolio Folio { get; }
    /// <summary>Gets the issue date of the referenced document.</summary>
    public DateOnly Date { get; }
    /// <summary>Gets the reference reason code (1 = Cancel, 2 = Correct text, 3 = Correct amounts).</summary>
    public byte ReferenceCode { get; }
    /// <summary>Gets the optional descriptive reason for the reference.</summary>
    public string? Reason { get; }

    private DocumentReference(DteTypeCode targetType, FiscalFolio folio, DateOnly date, byte referenceCode, string? reason)
    {
        TargetType = targetType;
        Folio = folio;
        Date = date;
        ReferenceCode = referenceCode;
        Reason = reason;
    }

    /// <summary>
    /// Creates a validated <see cref="DocumentReference"/> instance.
    /// </summary>
    /// <param name="targetType">The DTE document type being referenced.</param>
    /// <param name="folio">The fiscal folio number of the referenced document.</param>
    /// <param name="date">The emission date of the referenced document.</param>
    /// <param name="referenceCode">The reason code: 1 (Anula), 2 (Corrige Texto), or 3 (Corrige Montos).</param>
    /// <param name="reason">An optional free-text reason for the reference.</param>
    /// <returns>A <see cref="Result{DocumentReference}"/> containing the validated instance or a domain validation error.</returns>
    public static Result<DocumentReference> Create(DteTypeCode targetType, FiscalFolio folio, DateOnly date, byte referenceCode, string? reason = null)
    {
        if (referenceCode is not (1 or 2 or 3))
        {
            return Result<DocumentReference>.Failure(Error.Validation(
                "DocumentReference.InvalidReferenceCode", "The reference code must be 1 (Cancel), 2 (Correct Text), or 3 (Correct Amounts)."));
        }

        return Result<DocumentReference>.Success(new DocumentReference(targetType, folio, date, referenceCode, reason));
    }

    /// <inheritdoc/>
    public override string ToString() => $"DTE {TargetType.Code.ToString(CultureInfo.InvariantCulture)} Folio {Folio.Value.ToString(CultureInfo.InvariantCulture)} ({Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}) CodRef: {ReferenceCode.ToString(CultureInfo.InvariantCulture)}";
}




