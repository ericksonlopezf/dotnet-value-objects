// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a barcode (EAN, UPC, GS1-128, Data Matrix, etc.).
/// Alphanumeric to support industrial formats beyond digits-only.
/// </summary>
public sealed partial record Barcode : StringValueObject<Barcode>
{
    [GeneratedRegex(@"^[A-Z0-9][A-Z0-9 .-]{2,79}$")]
    private static partial Regex BarcodePattern();

    private Barcode(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Barcode"/> instance after validating and normalizing the input.
    /// </summary>
    /// <param name="value">The raw barcode string representation.</param>
    /// <returns>A <see cref="Result{Barcode}"/> containing the created instance or a validation error.</returns>
    public static Result<Barcode> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(Barcode), 3, 80,
            static n => new Barcode(n), StringPipeline.NormalizeTrimUpper,
            BarcodePattern(),
            "Barcode can contain uppercase letters, digits, spaces, periods, or hyphens.");
    }
}


