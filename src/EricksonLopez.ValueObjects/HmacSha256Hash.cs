// Copyright © Erickson Lopez. MIT License.
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a cryptographically secure 64-character hexadecimal HMAC-SHA256 hash (e.g. for API key signatures or webhook payloads).
/// Invariant: Must be exactly 64 hexadecimal characters, normalized to lowercase.
/// Masked by default via <see cref="SensitiveDataAttribute"/> and <see cref="object.ToString()"/>.
/// </summary>
[SensitiveData(mask: "****************************************************************")]
[DebuggerDisplay("{" + nameof(ToString) + "()}")]
public sealed partial record HmacSha256Hash : StringValueObject<HmacSha256Hash>
{
    [GeneratedRegex(@"^[a-f0-9]{64}$")]
    private static partial Regex Hex64Regex();

    /// <inheritdoc/>
    protected override bool IsSensitive => true;

    /// <inheritdoc/>
    protected override string Mask => "****************************************************************";

    private HmacSha256Hash(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="HmacSha256Hash"/> instance after validating format and length.
    /// </summary>
    /// <param name="value">The raw HMAC-SHA256 hash hexadecimal string.</param>
    /// <returns>A <see cref="Result{HmacSha256Hash}"/> containing the created instance or a validation error.</returns>
    public static Result<HmacSha256Hash> Create(string? value)
    {
        return StringPipeline.Required(
            value,
            nameof(HmacSha256Hash),
            64,
            64,
            static n => new HmacSha256Hash(n),
            static raw => raw.Trim().ToLowerInvariant(),
            Hex64Regex(),
            "HMAC-SHA256 hash must be exactly 64 hexadecimal characters.");
    }

    /// <summary>
    /// Hydrates an instance from trusted persistence storage without re-validation.
    /// </summary>
    /// <param name="value">The valid 64-character hex hash string.</param>
    /// <returns>The hydrated <see cref="HmacSha256Hash"/> instance.</returns>
    internal static HmacSha256Hash Hydrate(string value) => new(value);
}
