// Copyright © Erickson Lopez. MIT License.
using System;
using EricksonLopez.Result;

namespace EricksonLopez.ValueObjects;

/// <summary>
/// Represents a rich or multiline message body for emails, CRM communications, and notifications.
///
/// <para><b>Rules:</b> Required, 1–20,000 characters. Allows standard multiline whitespace (newlines, tabs) while rejecting non-printable control characters.</para>
/// <para><b>Used by:</b> CRM, Email Communications, Customer Service, Notifications</para>
/// </summary>
public sealed record MessageBody : StringValueObject<MessageBody>
{
    private MessageBody(string value) : base(value) { }

    /// <summary>
    /// Creates a new <see cref="MessageBody"/> instance after trimming and validating.
    /// </summary>
    /// <param name="value">The raw message body text.</param>
    /// <returns>A <see cref="Result{MessageBody}"/> containing the created instance or a validation error.</returns>
    public static Result<MessageBody> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<MessageBody>.Failure(Error.Validation(
                "MessageBody.Required", "Message body is required."));
        }

        string trimmed = value.Trim();

        if (ContainsInvalidControlCharacters(trimmed))
        {
            return Result<MessageBody>.Failure(Error.Validation(
                "MessageBody.ControlCharacters", "Message body contains invalid control characters."));
        }

        if (trimmed.Length > 20000)
        {
            return Result<MessageBody>.Failure(Error.Validation(
                "MessageBody.TooLong", "Message body must not exceed 20,000 characters."));
        }

        return Result<MessageBody>.Success(new MessageBody(trimmed));
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


