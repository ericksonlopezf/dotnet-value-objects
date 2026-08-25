// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an international passport identifier.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 5–20 uppercase alphanumeric characters.</para>
/// <para><b>Used by:</b> Human Resources, KYC, Travel, CRM, Immigration, Global Identity</para>
/// </summary>
[SensitiveData(mask: "XXXXXXXXX")]
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public sealed partial record PassportNumber : StringValueObject<PassportNumber>
{
    /// <inheritdoc/>
    protected override bool IsSensitive => true;
    /// <inheritdoc/>
    protected override string Mask => "XXXXXXXXX";

    [GeneratedRegex(@"^[A-Z0-9]{5,20}$")]
    private static partial Regex PassportPattern();

    private PassportNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="PassportNumber"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw passport number string.</param>
    /// <returns>A <see cref="Result{PassportNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<PassportNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(PassportNumber), 5, 20,
            static n => new PassportNumber(n), StringPipeline.NormalizeTrimUpper,
            PassportPattern(),
            "Passport number must contain between 5 and 20 alphanumeric characters.");
    }
}


