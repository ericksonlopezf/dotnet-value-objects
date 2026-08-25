// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a commercial sales channel identifier (e.g., "WEB", "POS_STORE_01", "MOBILE_APP", "B2B_PORTAL").
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–40 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> POS, E-commerce, ERP, Omnichannel Sales, Analytics</para>
/// </summary>
public sealed record SalesChannelCode : StringValueObject<SalesChannelCode>
{
    private SalesChannelCode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="SalesChannelCode"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw sales channel code string.</param>
    /// <returns>A <see cref="Result{SalesChannelCode}"/> containing the created instance or a validation error.</returns>
    public static Result<SalesChannelCode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(SalesChannelCode), 1, 40,
            static n => new SalesChannelCode(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Sales channel code must contain uppercase alphanumeric characters and standard separators.");
    }
}


