// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a collaborative comment or remark on tasks, tickets, and domain entities.
///
/// <para><b>Rules:</b> Required, 1–5,000 characters. Allows standard multiline whitespace (newlines, tabs) while rejecting non-printable control characters.</para>
/// <para><b>Used by:</b> CRM, Project Management, Issue Tracking, Social Feeds, Approvals</para>
/// </summary>
public sealed record Comment : StringValueObject<Comment>
{
    private Comment(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="Comment"/> instance after trimming and validating.
    /// </summary>
    /// <param name="value">The raw comment text.</param>
    /// <returns>A <see cref="Result{Comment}"/> containing the created instance or a validation error.</returns>
    public static Result<Comment> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<Comment>.Failure(Error.Validation(
                "Comment.Required", "Comment is required."));
        }

        string trimmed = value.Trim();

        if (ContainsInvalidControlCharacters(trimmed))
        {
            return Result<Comment>.Failure(Error.Validation(
                "Comment.ControlCharacters", "Comment contains invalid control characters."));
        }

        if (trimmed.Length > 5000)
        {
            return Result<Comment>.Failure(Error.Validation(
                "Comment.TooLong", "Comment must not exceed 5,000 characters."));
        }

        return Result<Comment>.Success(new Comment(trimmed));
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


