// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a generic national identity document number for any country.
/// Case is normalized to uppercase; format varies by country.
///
/// <para>For Dominican Cedula (11 digits) or RNC (9 digits), use <c>Rnc</c>
/// from <c>EricksonLopez.ValueObjects.Fiscal.DominicanRepublic</c>.</para>
///
/// <para><b>Rules:</b> Required, 1–30 chars, alphanumeric with hyphens, uppercase.</para>
/// <para><b>Used by:</b> ERP, Payroll, CRM, HR, Property Management, KYC</para>
/// <para><b>PII Protection:</b> <c>ToString()</c> returns the mask string configured in
/// <see cref="SensitiveDataAttribute"/> to prevent leakage in logs and traces.
/// Access the raw value explicitly via the <c>Value</c> property.</para>
/// </summary>
[SensitiveData(mask: "***")]
public sealed record NationalId : StringValueObject<NationalId>
{
    /// <inheritdoc/>
    protected override bool IsSensitive => true;

    private NationalId(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="NationalId"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw national identity document number.</param>
    /// <returns>A <see cref="Result{NationalId}"/> containing the created instance or a validation error.</returns>
    public static Result<NationalId> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(NationalId), 4, 40,
            static n => new NationalId(n),
            StringPipeline.NormalizeTrimUpper,
            StringPipeline.LooseIdentifierPattern,
            "National ID must contain alphanumeric characters, spaces, periods, underscores, slashes, or hyphens.");
    }
}


