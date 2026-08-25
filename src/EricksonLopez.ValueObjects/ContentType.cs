// Copyright © Erickson Lopez. MIT License.
using System;
using System.Text.RegularExpressions;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a MIME content type (e.g. "application/pdf", "image/png").
/// <para><b>Rules:</b> 3–255 chars, lowercase media/subtype format.</para>
/// <para><b>Used by:</b> Document management, uploads, APIs</para>
/// </summary>
public sealed partial record ContentType : StringValueObject<ContentType>
{
    [GeneratedRegex(@"^[a-z0-9][a-z0-9.+-]{0,126}/[a-z0-9][a-z0-9.+-]{0,126}$")]
    private static partial Regex ContentTypePattern();

    private ContentType(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="ContentType"/> instance after validating and normalizing the MIME type to lowercase.
    /// </summary>
    /// <param name="value">The raw MIME content type string.</param>
    /// <returns>A <see cref="Result{ContentType}"/> containing the created instance or a validation error.</returns>
    public static Result<ContentType> Create(string? value)
    {
        return StringPipeline.Required(value, nameof(ContentType), 3, 255,
            static n => new ContentType(n), StringPipeline.NormalizeLower,
            ContentTypePattern(), "Content type must use the media-type/subtype format.");
    }
}



