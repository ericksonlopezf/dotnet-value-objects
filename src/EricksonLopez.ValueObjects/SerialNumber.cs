// Copyright © Erickson Lopez. MIT License.
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a unique manufacturing or inventory item serial number.
/// Normalized to uppercase.
///
/// <para><b>Rules:</b> Required, 1–80 characters, uppercase alphanumeric with separators (<c>. _ / -</c>).</para>
/// <para><b>Used by:</b> Inventory, Fixed Assets, Manufacturing, Warranty, After-Sales Service</para>
/// </summary>
public sealed record SerialNumber : StringValueObject<SerialNumber>
{
    private SerialNumber(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="SerialNumber"/> instance after validating and normalizing to uppercase.
    /// </summary>
    /// <param name="value">The raw serial number string.</param>
    /// <returns>A <see cref="Result{SerialNumber}"/> containing the created instance or a validation error.</returns>
    public static Result<SerialNumber> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(SerialNumber), 1, 80,
            static n => new SerialNumber(n), StringPipeline.NormalizeCode,
            StringPipeline.CodePattern,
            "Serial number must contain uppercase alphanumeric characters and standard separators.");
    }
}


