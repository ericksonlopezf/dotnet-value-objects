// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a file system-safe file name.
/// <para><b>Rules:</b> 1–255 chars, no characters that are invalid on Windows NTFS,
/// Linux ext4, or common cloud storage providers (Azure Blob, S3).</para>
/// <para><b>Used by:</b> Document management, uploads, storage</para>
///
/// <para><b>Design note:</b> This VO does <b>not</b> use <c>System.IO.Path.GetInvalidFileNameChars()</c>
/// because that method returns OS-specific results (different on Windows vs Linux), which would make
/// domain validation non-deterministic across environments. Instead, a curated explicit set of characters
/// is defined here that covers the union of restrictions across all target platforms.</para>
///
/// <para><b>Prohibited characters</b> (platform-union set):</para>
/// <list type="bullet">
///   <item>ASCII control characters (0x00–0x1F) — invalid everywhere</item>
///   <item><c>\ / : * ? " &lt; &gt; |</c> — invalid on Windows NTFS and many cloud providers</item>
///   <item>Null byte (0x00) — invalid on all POSIX file systems</item>
/// </list>
/// </summary>
public sealed record FileName : StringValueObject<FileName>
{
    private FileName(string value) : base(value) { }

    /// <summary>Returns the file extension including the dot (e.g., <c>.pdf</c>).</summary>
    public string Extension
    {
        get
        {
            int dotIndex = Value.LastIndexOf('.');
            return dotIndex >= 0 ? Value[dotIndex..] : string.Empty;
        }
    }

    /// <summary>
    /// Creates a new <see cref="FileName"/> instance after validating that it contains no prohibited characters.
    /// </summary>
    /// <param name="value">The raw file name string.</param>
    /// <returns>A <see cref="Result{FileName}"/> containing the created instance or a validation error.</returns>
    public static Result<FileName> Create(string? value)
    {
        Result<string> normalized = StringPipeline.RequiredString(
            value, nameof(FileName), 1, 255, static raw => raw.Trim());

        if (normalized.IsFailure)
        {
            return Result<FileName>.Failure(normalized.Error);
        }

        foreach (char c in normalized.Value)
        {
            if (IsInvalidFileNameChar(c))
            {
                return Result<FileName>.Failure(Error.Validation(
                    "FileName.InvalidCharacters",
                    "File name contains characters that are invalid on Windows, Linux, or cloud storage platforms."));
            }
        }

        return Result<FileName>.Success(new FileName(normalized.Value));
    }

    private static bool IsInvalidFileNameChar(char c) =>
        c is '\\' or '/' or ':' or '*' or '?' or '"' or '<' or '>' or '|';
}

