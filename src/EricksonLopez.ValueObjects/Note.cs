// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents an internal administrative note, audit comment, or operational remark.
///
/// <para><b>Rules:</b> Required, 1–5,000 characters. Allows standard multiline whitespace (newlines, tabs) while rejecting non-printable control characters.</para>
/// <para><b>Used by:</b> ERP, Audit, Customer Support, Project Tasks, Collections</para>
/// </summary>
public sealed record Note : StringValueObject<Note>
{
    private Note(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Note"/> instance after trimming and validating.
    /// </summary>
    /// <param name="value">The raw note text.</param>
    /// <returns>A <see cref="Result{Note}"/> containing the created instance or a validation error.</returns>
    public static Result<Note> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Note>.Failure(Error.Validation(
                "Note.Required", "Note is required."));
        }

        string trimmed = value.Trim();

        if (ContainsInvalidControlCharacters(trimmed))
        {
            return Result<Note>.Failure(Error.Validation(
                "Note.ControlCharacters", "Note contains invalid control characters."));
        }

        if (trimmed.Length > 5000)
        {
            return Result<Note>.Failure(Error.Validation(
                "Note.TooLong", "Note must not exceed 5,000 characters."));
        }

        return Result<Note>.Success(new Note(trimmed));
    }

    private static bool ContainsInvalidControlCharacters(string value)
    {
        foreach (char c in value)
        {
            if (char.IsControl(c) && c is not '\r' and not '\n' and not '\t')
            {
                return true;
            }
        }
        return false;
    }
}


